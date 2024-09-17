using Application.Commands.CreateUserInterest;
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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using static Application.Services.CheckingBadWord;

namespace Application.Commands.Post
{
    public class CreateUserPostCommandHandler : ICommandHandler<CreateUserPostCommand, CreateUserPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public CreateUserPostCommandHandler(fptforumCommandContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<CreateUserPostCommandResult>> Handle(CreateUserPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var photos = request.Photos ?? new List<PhotoAddOnPost>();
            var videos = request.Videos ?? new List<VideoAddOnPost>();

            Guid PhotoIdSingle = Guid.Empty;
            Guid VideoIdSingle = Guid.Empty;
            int numberPost = photos.Count() + videos.Count();
            if (photos.Count() == 1 && numberPost == 1)
            {
                PhotoIdSingle = await UploadImage(photos.First(), request.UserId, request.UserStatusId);
            }
            if (videos.Count() == 1 && numberPost == 1)
            {
                VideoIdSingle = await UploadVideo(videos.First(), request.UserId, request.UserStatusId);
            }


            Domain.CommandModels.UserPost userPost = new Domain.CommandModels.UserPost
            {
                UserPostId = _helper.GenerateNewGuid(),
                UserId = request.UserId,
                Content = request.Content,
                UserPostNumber = DateTime.Now.ToString("ddMMyyHHmmss") + request.UserId.ToString().Replace("-", ""),
                UserStatusId = request.UserStatusId,
                IsAvataPost = request.IsAvataPost,
                IsCoverPhotoPost = request.IsCoverPhotoPost,
                IsHide = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                NumberPost = numberPost,
                IsBanned = false,
            };
            if (PhotoIdSingle != Guid.Empty)
            {
                userPost.PhotoId = PhotoIdSingle;
            }

            if (VideoIdSingle != Guid.Empty)
            {
                userPost.VideoId = VideoIdSingle;
            }
            await _context.UserPosts.AddAsync(userPost);
            await _context.SaveChangesAsync();
            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(userPost.Content);
            if (haveBadWord.Any())
            {
                userPost.IsBanned = true;
                userPost.Content = _checkContent.MarkBannedWordsInContent(userPost.Content, haveBadWord);
                await _context.SaveChangesAsync();
            }

            Domain.CommandModels.PostReactCount postReactCount = new Domain.CommandModels.PostReactCount
            {
                PostReactCountId = _helper.GenerateNewGuid(),
                UserPostId = userPost.UserPostId,
                ReactCount = 0,
                CommentCount = 0,
                ShareCount = 0,
                CreateAt = DateTime.Now
            };
            await _context.PostReactCounts.AddAsync(postReactCount);
            await _context.SaveChangesAsync();

            if (numberPost > 1)
            {
                foreach (var photo in photos)
                {
                    Guid photoId = await UploadImage(photo, request.UserId, request.UserStatusId);
                    Domain.CommandModels.UserPostPhoto userPostPhoto = new Domain.CommandModels.UserPostPhoto
                    {
                        UserPostPhotoId = _helper.GenerateNewGuid(),
                        UserPostId = userPost.UserPostId,
                        PhotoId = photoId,
                        Content = string.Empty,
                        UserPostPhotoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                        UserStatusId = userPost.UserStatusId,
                        IsHide = userPost.IsHide,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        PostPosition = photo.Position
                    };
                    await _context.UserPostPhotos.AddAsync(userPostPhoto);
                    await _context.SaveChangesAsync();

                    Domain.CommandModels.PostReactCount photoPostReactCount = new Domain.CommandModels.PostReactCount
                    {
                        PostReactCountId = _helper.GenerateNewGuid(),
                        UserPostPhotoId = userPostPhoto.UserPostPhotoId,
                        ReactCount = 0,
                        CommentCount = 0,
                        ShareCount = 0,
                        CreateAt = DateTime.Now
                    };
                    await _context.PostReactCounts.AddAsync(photoPostReactCount);
                    await _context.SaveChangesAsync();
                }

                foreach (var video in videos)
                {
                    Guid videoId = await UploadVideo(video, request.UserId, request.UserStatusId);
                    Domain.CommandModels.UserPostVideo userPostVideo = new Domain.CommandModels.UserPostVideo
                    {
                        UserPostVideoId = _helper.GenerateNewGuid(),
                        UserPostId = userPost.UserPostId,
                        VideoId = videoId,
                        Content = string.Empty,
                        UserPostVideoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                        UserStatusId = userPost.UserStatusId,
                        IsHide = userPost.IsHide,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        PostPosition = video.Position,
                    };
                    await _context.UserPostVideos.AddAsync(userPostVideo);
                    await _context.SaveChangesAsync();

                    Domain.CommandModels.PostReactCount videoPostReactCount = new Domain.CommandModels.PostReactCount
                    {
                        PostReactCountId = _helper.GenerateNewGuid(),
                        UserPostVideoId = userPostVideo.UserPostVideoId,
                        ReactCount = 0,
                        CommentCount = 0,
                        ShareCount = 0,
                        CreateAt = DateTime.Now
                    };
                    await _context.PostReactCounts.AddAsync(videoPostReactCount);
                    await _context.SaveChangesAsync();

                }
            }
            var result = _mapper.Map<CreateUserPostCommandResult>(userPost);
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<CreateUserPostCommandResult>.Success(result);
            }
        }

        private async Task<Guid> UploadImage(PhotoAddOnPost photo, Guid userId, Guid userStatusId)
        {
            var photoEntity = new Domain.CommandModels.Photo
            {
                PhotoId = _helper.GenerateNewGuid(),
                UserId = userId,
                PhotoUrl = photo.PhotoUrl,
                UserStatusId = userStatusId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Photos.AddAsync(photoEntity);
            await _context.SaveChangesAsync();

            return photoEntity.PhotoId;
        }

        private async Task<Guid> UploadVideo(VideoAddOnPost video, Guid userId, Guid userStatusId)
        {
            var videoEntity = new Domain.CommandModels.Video
            {
                VideoId = _helper.GenerateNewGuid(),
                UserId = userId,
                VideoUrl = video.VideoUrl,
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
