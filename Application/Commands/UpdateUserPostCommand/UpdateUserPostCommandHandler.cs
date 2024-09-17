using Application.Commands.Post;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Services;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Application.Services.CheckingBadWord;

namespace Application.Commands.UpdateUserPostCommand
{
    public class UpdateUserPostCommandHandler : ICommandHandler<UpdateUserPostCommand, UpdateUserPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateUserPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<UpdateUserPostCommandResult>> Handle(UpdateUserPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userPost = _querycontext.UserPosts.Where(x => x.UserPostId == request.UserPostId).AsNoTracking().FirstOrDefault();
            if (userPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }

            if (request.UserId != userPost.UserId)
            {
                throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
            }

            var photos = request.Photos ?? new List<PhotoAddOnPost>();
            var videos = request.Videos ?? new List<VideoAddOnPost>();

            var existingPhotos = await _querycontext.UserPostPhotos
                                                .Where(p => p.UserPostId == userPost.UserPostId && p.IsHide != true)
                                                .Include(p => p.Photo)
                                                .ToListAsync();
            var existingPhotoUrls = existingPhotos.Select(p => p.Photo.PhotoUrl).ToList();

            var existingVideos = await _querycontext.UserPostVideos
                                                .Where(v => v.UserPostId == userPost.UserPostId && v.IsHide != true)
                                                .Include(p => p.Video)
                                                .ToListAsync();
            var existingVideoUrls = existingVideos.Select(v => v.Video.VideoUrl).ToList();


            var newPhotoUrls = photos.Select(p => p.PhotoUrl).ToList();
            var newVideoUrls = videos.Select(v => v.VideoUrl).ToList();

            var newPhotos = newPhotoUrls.Except(existingPhotoUrls).ToList();
            var newVideos = newVideoUrls.Except(existingVideoUrls).ToList();

            Guid PhotoIdSingle = Guid.Empty;
            Guid VideoIdSingle = Guid.Empty;
            int numberPost = photos.Count() + videos.Count();
            if (numberPost == 1)
            {
                if (photos.Count() == 1 && numberPost == 1)
                {
                    PhotoIdSingle = await UploadImage(photos.First().PhotoUrl, request.UserId, request.UserStatusId);
                    
                }
                if (videos.Count() == 1 && numberPost == 1)
                {
                    VideoIdSingle = await UploadVideo(videos.First().VideoUrl, request.UserId, request.UserStatusId);
                }
                //Chuyển tất cả ảnh đang tồn tại trong UserPostPhoto và Video về IsHide = true
                foreach (var photo in existingPhotos)
                {
                    var updatePhoto = new Domain.CommandModels.UserPostPhoto
                    {
                        UserPostPhotoId = photo.UserPostPhotoId,
                        UserPostId = photo.UserPostId,
                        PhotoId = photo.PhotoId,
                        Content = photo.Content,
                        UserPostPhotoNumber = photo.UserPostPhotoNumber,
                        UserStatusId = photo.UserStatusId,
                        IsHide = true,
                        CreatedAt = photo.CreatedAt,
                        UpdatedAt = DateTime.Now,
                        PostPosition = photo.PostPosition,
                    };
                    _context.UserPostPhotos.Update(updatePhoto);
                    _context.SaveChanges();
                }

                foreach (var video in existingVideos)
                {
                    var updateVideo = new Domain.CommandModels.UserPostVideo
                    {
                        UserPostVideoId = video.UserPostVideoId,
                        UserPostId = video.UserPostId,
                        VideoId = video.VideoId,
                        Content = video.Content,
                        UserPostVideoNumber = video.UserPostVideoNumber,
                        UserStatusId = video.UserStatusId,
                        IsHide = true,
                        CreatedAt = video.CreatedAt,
                        UpdatedAt = DateTime.Now,
                        PostPosition = video.PostPosition,
                    };
                    _context.UserPostVideos.Update(updateVideo);
                    _context.SaveChanges();
                }


                if (PhotoIdSingle != Guid.Empty)
                {
                    userPost.PhotoId = PhotoIdSingle;
                }

                if (VideoIdSingle != Guid.Empty)
                {
                    userPost.VideoId = VideoIdSingle;
                }
            }
            else
            {
                foreach (var photo in photos)
                {
                    var existingPhoto = existingPhotos
                        .FirstOrDefault(p => p.Photo.PhotoUrl == photo.PhotoUrl);

                    if (existingPhoto != null)
                    {
                        // Ảnh đã tồn tại, chỉ cần cập nhật lại Position
                        var updatePhoto = new Domain.CommandModels.UserPostPhoto
                        {
                            UserPostPhotoId = existingPhoto.UserPostPhotoId,
                            UserPostId = existingPhoto.UserPostId,
                            PhotoId = existingPhoto.PhotoId,
                            Content = existingPhoto.Content,
                            UserPostPhotoNumber = existingPhoto.UserPostPhotoNumber,
                            UserStatusId = request.UserStatusId,
                            IsHide = existingPhoto.IsHide,
                            CreatedAt = existingPhoto.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            PostPosition = photo.Position,
                        };
                        _context.UserPostPhotos.Update(updatePhoto);
                    }
                    else
                    {
                        // Ảnh mới, tạo mới
                        Guid photoId = await UploadImage(photo.PhotoUrl, userPost.UserId, userPost.UserStatusId);
                        var newUserPostPhoto = new Domain.CommandModels.UserPostPhoto
                        {
                            UserPostPhotoId = _helper.GenerateNewGuid(),
                            UserPostId = userPost.UserPostId,
                            PhotoId = photoId,
                            Content = string.Empty,
                            UserPostPhotoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                            UserStatusId = request.UserStatusId,
                            IsHide = userPost.IsHide,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            PostPosition = photo.Position,
                        };
                        await _context.UserPostPhotos.AddAsync(newUserPostPhoto);

                        Domain.CommandModels.PostReactCount photoPostReactCount = new Domain.CommandModels.PostReactCount
                        {
                            PostReactCountId = _helper.GenerateNewGuid(),
                            UserPostPhotoId = newUserPostPhoto.UserPostPhotoId,
                            ReactCount = 0,
                            CommentCount = 0,
                            ShareCount = 0,
                            CreateAt = DateTime.Now
                        };
                        await _context.PostReactCounts.AddAsync(photoPostReactCount);
                    }
                }

                foreach (var video in videos)
                {
                    var existingVideo = existingVideos
                        .FirstOrDefault(v => v.Video.VideoUrl == video.VideoUrl);

                    if (existingVideo != null)
                    {
                        var updateVideo = new Domain.CommandModels.UserPostVideo
                        {
                            UserPostVideoId = existingVideo.UserPostVideoId,
                            UserPostId = existingVideo.UserPostId,
                            VideoId = existingVideo.VideoId,
                            Content = existingVideo.Content,
                            UserPostVideoNumber = existingVideo.UserPostVideoNumber,
                            UserStatusId = request.UserStatusId,
                            IsHide = existingVideo.IsHide,
                            CreatedAt = existingVideo.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            PostPosition = video.Position,
                        };
                        _context.UserPostVideos.Update(updateVideo);
                    }
                    else
                    {
                        // Video mới, tạo mới
                        Guid videoId = await UploadVideo(video.VideoUrl, userPost.UserId, userPost.UserStatusId);
                        var newUserPostVideo = new Domain.CommandModels.UserPostVideo
                        {
                            UserPostVideoId = _helper.GenerateNewGuid(),
                            UserPostId = userPost.UserPostId,
                            VideoId = videoId,
                            Content = string.Empty,
                            UserPostVideoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                            UserStatusId = request.UserStatusId,
                            IsHide = userPost.IsHide,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            PostPosition = video.Position
                        };
                        await _context.UserPostVideos.AddAsync(newUserPostVideo);

                        Domain.CommandModels.PostReactCount videoPostReactCount = new Domain.CommandModels.PostReactCount
                        {
                            PostReactCountId = _helper.GenerateNewGuid(),
                            UserPostVideoId = newUserPostVideo.UserPostVideoId,
                            ReactCount = 0,
                            CommentCount = 0,
                            ShareCount = 0,
                            CreateAt = DateTime.Now
                        };
                        await _context.PostReactCounts.AddAsync(videoPostReactCount);
                    }
                }

                // Ẩn các ảnh không có trong danh sách cập nhật
                foreach (var existingPhoto in existingPhotos)
                {
                    if (!newPhotoUrls.Contains(existingPhoto.Photo.PhotoUrl))
                    {
                        var newUserPostPhoto = new Domain.CommandModels.UserPostPhoto
                        {
                            UserPostPhotoId = existingPhoto.UserPostPhotoId,
                            UserPostId = existingPhoto.UserPostId,
                            PhotoId = existingPhoto.PhotoId,
                            Content = existingPhoto.Content,
                            UserPostPhotoNumber = existingPhoto.UserPostPhotoNumber,
                            UserStatusId = existingPhoto.UserStatusId,
                            IsHide = true,
                            CreatedAt = existingPhoto.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            PostPosition = existingPhoto.PostPosition,
                        };
                        _context.UserPostPhotos.Update(newUserPostPhoto);
                    }
                }

                // Ẩn các video không có trong danh sách cập nhật
                foreach (var existingVideo in existingVideos)
                {
                    if (!newVideoUrls.Contains(existingVideo.Video.VideoUrl))
                    {
                        var newUserPostVideo = new Domain.CommandModels.UserPostVideo
                        {
                            UserPostVideoId = existingVideo.UserPostVideoId,
                            UserPostId = existingVideo.UserPostId,
                            VideoId = existingVideo.VideoId,
                            Content = existingVideo.Content,
                            UserPostVideoNumber = existingVideo.UserPostVideoNumber,
                            UserStatusId = existingVideo.UserStatusId,
                            IsHide = true,
                            CreatedAt = existingVideo.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            PostPosition = existingVideo.PostPosition,
                        };
                        _context.UserPostVideos.Update(newUserPostVideo);
                    }
                }

                if (userPost.PhotoId != null) 
                {
                    userPost.PhotoId = null;
                }

                if (userPost.VideoId != null)
                {
                    userPost.VideoId = null;
                }

                await _context.SaveChangesAsync();
            }

            userPost.IsBanned = false;
            userPost.Content = request.Content;
            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(request.Content);
            if (haveBadWord.Any())
            {
                userPost.IsBanned = true;
                userPost.Content = _checkContent.MarkBannedWordsInContent(request.Content, haveBadWord);
            }
            userPost.UserStatusId = request.UserStatusId;
            userPost.UpdatedAt = DateTime.Now;
            userPost.NumberPost = numberPost;

            Domain.CommandModels.UserPost up = new Domain.CommandModels.UserPost
            {
                UserPostId = userPost.UserPostId,
                UserId = userPost.UserId,
                Content = userPost.Content,
                UserPostNumber = userPost.UserPostNumber,
                UserStatusId = userPost.UserStatusId,
                IsAvataPost = userPost.IsAvataPost,
                IsCoverPhotoPost = userPost.IsCoverPhotoPost,
                IsHide = userPost.IsHide,
                CreatedAt = userPost.CreatedAt,
                UpdatedAt = DateTime.Now,
                PhotoId = userPost.PhotoId,
                VideoId = userPost.VideoId,
                NumberPost = userPost.NumberPost,
                IsBanned = userPost.IsBanned,
            };
            _context.UserPosts.Update(up);
            _context.SaveChanges();

            var result = _mapper.Map<UpdateUserPostCommandResult>(userPost);
            result.BannedWords = new List<BannedWord>();
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<UpdateUserPostCommandResult>.Success(result);
            }
        }

        private async Task<Guid> UploadImage(string photoUrl, Guid userId, Guid userStatusId)
        {
            var photoEntity = new Domain.CommandModels.Photo
            {
                PhotoId = _helper.GenerateNewGuid(),
                UserId = userId,
                PhotoUrl = photoUrl,
                UserStatusId = userStatusId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Photos.AddAsync(photoEntity);
            await _context.SaveChangesAsync();

            return photoEntity.PhotoId;
        }

        private async Task<Guid> UploadVideo(string videoUrl, Guid userId, Guid userStatusId)
        {
            var videoEntity = new Domain.CommandModels.Video
            {
                VideoId = _helper.GenerateNewGuid(),
                UserId = userId,
                VideoUrl = videoUrl,
                UserStatusId = userStatusId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Videos.AddAsync(videoEntity);
            await _context.SaveChangesAsync();

            return videoEntity.VideoId;
        }
    }
}
