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
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Application.Services.CheckingBadWord;

namespace Application.Commands.UpdateGroupPostCommand
{
    public class UpdateGroupPostCommandHandler : ICommandHandler<UpdateGroupPostCommand, UpdateGroupPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateGroupPostCommandHandler(fptforumCommandContext context, fptforumQueryContext queryContext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _querycontext = queryContext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<UpdateGroupPostCommandResult>> Handle(UpdateGroupPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var GroupPost = await _querycontext.GroupPosts.FindAsync(request.GroupPostId);
            if (GroupPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            if (request.UserId != GroupPost.UserId)
            {
                throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
            }

            var photos = request.Photos ?? new List<PhotoAddOnPost>();
            var videos = request.Videos ?? new List<VideoAddOnPost>();

            var existingPhotos = await _querycontext.GroupPostPhotos
                                                .Where(p => p.GroupPostId == GroupPost.GroupPostId && p.IsHide != true)
                                                .Include(p => p.GroupPhoto)
                                                .ToListAsync();

            var existingPhotoUrls = existingPhotos.Select(p => p.GroupPhoto.PhotoUrl).ToList();

            var existingVideos = await _querycontext.GroupPostVideos
                                                .Where(p => p.GroupPostId == GroupPost.GroupPostId && p.IsHide != true)
                                                .Include(p => p.GroupVideo)
                                                .ToListAsync();

            var existingVideoUrls = existingVideos.Select(p => p.GroupVideo.VideoUrl).ToList();

            var newPhotoUrls = photos.Select(p => p.PhotoUrl).ToList();
            var newVideoUrls = videos.Select(v => v.VideoUrl).ToList();

            var newPhotos = newPhotoUrls.Except(existingPhotoUrls).ToList();
            var newVideos = newVideoUrls.Except(existingVideoUrls).ToList();

            Guid groupStatusId = (Guid)_context.GroupFpts.Where(x => x.GroupId == GroupPost.GroupId).Select(x => x.GroupStatusId).FirstOrDefault();

            Guid PhotoIdSingle = Guid.Empty;
            Guid VideoIdSingle = Guid.Empty;
            int numberPost = photos.Count() + videos.Count();
            if (numberPost == 1)
            {
                if (photos.Count() == 1 && numberPost == 1)
                {
                    PhotoIdSingle = await UploadImage(photos.First().PhotoUrl, (Guid)GroupPost.GroupId);
                }
                if (videos.Count() == 1 && numberPost == 1)
                {
                    VideoIdSingle = await UploadVideo(videos.First().VideoUrl, (Guid)GroupPost.GroupId);
                }

                foreach (var photo in existingPhotos)
                {
                    var updatePhoto = new Domain.CommandModels.GroupPostPhoto
                    {
                        GroupPostPhotoId = photo.GroupPostPhotoId,
                        GroupPostId = photo.GroupPostId,
                        GroupPhotoId = photo.GroupPhotoId,
                        Content = photo.Content,
                        GroupPostPhotoNumber = photo.GroupPostPhotoNumber,
                        GroupStatusId = groupStatusId,
                        IsHide = true,
                        CreatedAt = photo.CreatedAt,
                        UpdatedAt = DateTime.Now,
                        PostPosition = photo.PostPosition,
                    };
                    _context.GroupPostPhotos.Update(updatePhoto);
                    _context.SaveChanges();
                }

                foreach (var video in existingVideos)
                {
                    var updateVideo = new Domain.CommandModels.GroupPostVideo
                    {
                        GroupPostVideoId = video.GroupPostVideoId,
                        GroupPostId = video.GroupPostId,
                        GroupVideoId = video.GroupVideoId,
                        Content = video.Content,
                        GroupPostVideoNumber = video.GroupPostVideoNumber,
                        GroupStatusId = groupStatusId,
                        IsHide = true,
                        CreatedAt = video.CreatedAt,
                        UpdatedAt = DateTime.Now,
                        PostPosition = video.PostPosition,
                    };
                    _context.GroupPostVideos.Update(updateVideo);
                    _context.SaveChanges();
                }


                if (PhotoIdSingle != Guid.Empty)
                {
                    GroupPost.GroupPhotoId = PhotoIdSingle;
                }

                if (VideoIdSingle != Guid.Empty)
                {
                    GroupPost.GroupVideoId = VideoIdSingle;
                }
            }
            else
            {
                foreach (var photo in photos)
                {
                    var existingPhoto = existingPhotos
                        .FirstOrDefault(p => p.GroupPhoto.PhotoUrl == photo.PhotoUrl);

                    if (existingPhoto != null)
                    {
                        // Ảnh đã tồn tại, chỉ cần cập nhật lại Position
                        var updatePhoto = new Domain.CommandModels.GroupPostPhoto
                        {
                            GroupPostPhotoId = existingPhoto.GroupPostPhotoId,
                            GroupPostId = existingPhoto.GroupPostId,
                            GroupPhotoId = existingPhoto.GroupPhotoId,
                            Content = existingPhoto.Content,
                            GroupPostPhotoNumber = existingPhoto.GroupPostPhotoNumber,
                            GroupStatusId = existingPhoto.GroupStatusId,
                            IsHide = existingPhoto.IsHide,
                            CreatedAt = existingPhoto.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            PostPosition = photo.Position,
                        };
                        _context.GroupPostPhotos.Update(updatePhoto);
                    }
                    else
                    {
                        // Ảnh mới, tạo mới
                        Guid photoId = await UploadImage(photo.PhotoUrl, (Guid)GroupPost.GroupId);
                        var newGroupPostPhoto = new Domain.CommandModels.GroupPostPhoto
                        {
                            GroupPostPhotoId = _helper.GenerateNewGuid(),
                            GroupPostId = GroupPost.GroupPostId,
                            GroupPhotoId = photoId,
                            Content = string.Empty,
                            GroupPostPhotoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                            GroupStatusId = GroupPost.GroupStatusId,
                            IsHide = GroupPost.IsHide,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            PostPosition = photo.Position,
                        };
                        await _context.GroupPostPhotos.AddAsync(newGroupPostPhoto);

                        Domain.CommandModels.GroupPostReactCount photoPostReactCount = new Domain.CommandModels.GroupPostReactCount
                        {
                            GroupPostReactCountId = _helper.GenerateNewGuid(),
                            GroupPostPhotoId = newGroupPostPhoto.GroupPostPhotoId,
                            ReactCount = 0,
                            CommentCount = 0,
                            ShareCount = 0,
                        };
                        await _context.GroupPostReactCounts.AddAsync(photoPostReactCount);
                    }
                }
                foreach (var video in videos)
                {
                    var existingVideo = existingVideos
                        .FirstOrDefault(v => v.GroupVideo.VideoUrl == video.VideoUrl);

                    if (existingVideo != null)
                    {
                        var updateVideo = new Domain.CommandModels.GroupPostVideo
                        {
                            GroupPostVideoId = existingVideo.GroupPostVideoId,
                            GroupPostId = existingVideo.GroupPostId,
                            GroupVideoId = existingVideo.GroupVideoId,
                            Content = existingVideo.Content,
                            GroupPostVideoNumber = existingVideo.GroupPostVideoNumber,
                            GroupStatusId = existingVideo.GroupStatusId,
                            IsHide = existingVideo.IsHide,
                            CreatedAt = existingVideo.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            PostPosition = video.Position,
                        };
                        _context.GroupPostVideos.Update(updateVideo);
                    }
                    else
                    {
                        // Video mới, tạo mới
                        Guid videoId = await UploadVideo(video.VideoUrl, (Guid)GroupPost.GroupId);
                        var newGroupPostVideo = new Domain.CommandModels.GroupPostVideo
                        {
                            GroupPostVideoId = _helper.GenerateNewGuid(),
                            GroupPostId = GroupPost.GroupPostId,
                            GroupVideoId = videoId,
                            Content = string.Empty,
                            GroupPostVideoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                            GroupStatusId = GroupPost.GroupStatusId,
                            IsHide = GroupPost.IsHide,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            PostPosition = video.Position
                        };
                        await _context.GroupPostVideos.AddAsync(newGroupPostVideo);

                        Domain.CommandModels.GroupPostReactCount videoPostReactCount = new Domain.CommandModels.GroupPostReactCount
                        {
                            GroupPostReactCountId = _helper.GenerateNewGuid(),
                            GroupPostVideoId = newGroupPostVideo.GroupPostVideoId,
                            ReactCount = 0,
                            CommentCount = 0,
                            ShareCount = 0,
                        };
                        await _context.GroupPostReactCounts.AddAsync(videoPostReactCount);
                    }
                }

                // Ẩn các ảnh không có trong danh sách cập nhật
                foreach (var existingPhoto in existingPhotos)
                {
                    if (!newPhotoUrls.Contains(existingPhoto.GroupPhoto.PhotoUrl))
                    {
                        var newGroupPostPhoto = new Domain.CommandModels.GroupPostPhoto
                        {
                            GroupPostPhotoId = existingPhoto.GroupPostPhotoId,
                            GroupPostId = existingPhoto.GroupPostId,
                            GroupPhotoId = existingPhoto.GroupPhotoId,
                            Content = existingPhoto.Content,
                            GroupPostPhotoNumber = existingPhoto.GroupPostPhotoNumber,
                            GroupStatusId = existingPhoto.GroupStatusId,
                            IsHide = true,
                            CreatedAt = existingPhoto.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            PostPosition = existingPhoto.PostPosition,
                            GroupId = existingPhoto.GroupId,
                            IsBanned = existingPhoto.IsBanned,
                            IsPending = existingPhoto.IsPending,
                        };
                        _context.GroupPostPhotos.Update(newGroupPostPhoto);
                    }
                }

                // Ẩn các video không có trong danh sách cập nhật
                foreach (var existingVideo in existingVideos)
                {
                    if (!newVideoUrls.Contains(existingVideo.GroupVideo.VideoUrl))
                    {
                        var newGroupPostVideo = new Domain.CommandModels.GroupPostVideo
                        {
                            GroupPostVideoId = existingVideo.GroupPostVideoId,
                            GroupPostId = existingVideo.GroupPostId,
                            GroupVideoId = existingVideo.GroupVideoId,
                            Content = existingVideo.Content,
                            GroupPostVideoNumber = existingVideo.GroupPostVideoNumber,
                            GroupStatusId = existingVideo.GroupStatusId,
                            IsHide = true,
                            CreatedAt = existingVideo.CreatedAt,
                            UpdatedAt = DateTime.Now,
                            PostPosition = existingVideo.PostPosition,
                            GroupId = existingVideo.GroupId,
                            IsBanned = existingVideo.IsBanned,
                            IsPending = existingVideo.IsPending,
                        };
                        _context.GroupPostVideos.Update(newGroupPostVideo);
                    }
                }

                if (GroupPost.GroupPhotoId != null)
                {
                    GroupPost.GroupPhotoId = null;
                }

                if (GroupPost.GroupVideoId != null)
                {
                    GroupPost.GroupVideoId = null;
                }

                await _context.SaveChangesAsync();
            }
            GroupPost.IsBanned = false;
            GroupPost.Content = request.Content;
            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(request.Content);
            if (haveBadWord.Any())
            {
                GroupPost.IsBanned = true;
                GroupPost.Content = _checkContent.MarkBannedWordsInContent(request.Content, haveBadWord);
            }
            GroupPost.GroupStatusId = groupStatusId;
            GroupPost.UpdatedAt = DateTime.Now;
            GroupPost.NumberPost = numberPost;

            Domain.CommandModels.GroupPost gp1 = new Domain.CommandModels.GroupPost
            {
                GroupPostId = GroupPost.GroupPostId,
                UserId = GroupPost.UserId,
                Content = GroupPost.Content,
                GroupPostNumber = GroupPost.GroupPostNumber,
                GroupStatusId = groupStatusId,
                IsHide = GroupPost.IsHide,
                CreatedAt = GroupPost.CreatedAt,
                UpdatedAt = DateTime.Now,
                GroupPhotoId = GroupPost.GroupPhotoId,
                GroupVideoId = GroupPost.GroupVideoId,
                NumberPost = GroupPost.NumberPost,
                IsBanned = GroupPost.IsBanned,
                GroupId = GroupPost.GroupId,
                IsPending = GroupPost.IsPending,
            };
            _context.GroupPosts.Update(gp1);
            _context.SaveChanges();
            
            var result = _mapper.Map<UpdateGroupPostCommandResult>(gp1);
            result.BannedWords = new List<BannedWord>();
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<UpdateGroupPostCommandResult>.Success(result);
            }
        }

        private async Task<Guid> UploadImage(string photoUrl, Guid groupId)
        {
            var photoEntity = new Domain.CommandModels.GroupPhoto
            {
                GroupPhotoId = _helper.GenerateNewGuid(),
                GroupId = groupId,
                PhotoUrl = photoUrl,
                GroupPhotoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _context.GroupPhotos.AddAsync(photoEntity);
            await _context.SaveChangesAsync();

            return photoEntity.GroupPhotoId;
        }

        private async Task<Guid> UploadVideo(string videoUrl, Guid groupId)
        {
            var videoEntity = new Domain.CommandModels.GroupVideo
            {
                GroupVideoId = _helper.GenerateNewGuid(),
                GroupId = groupId,
                VideoUrl = videoUrl,
                GroupVideoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _context.GroupVideos.AddAsync(videoEntity);
            await _context.SaveChangesAsync();

            return videoEntity.GroupVideoId;
        }
    }
}
