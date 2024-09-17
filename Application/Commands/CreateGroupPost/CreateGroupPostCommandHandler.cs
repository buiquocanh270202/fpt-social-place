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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using static Application.Services.CheckingBadWord;

namespace Application.Commands.CreateGroupPost
{
    public class CreateGroupPostCommandHandler : ICommandHandler<CreateGroupPostCommand, CreateGroupPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public CreateGroupPostCommandHandler(fptforumCommandContext context, fptforumQueryContext queryContext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _queryContext = queryContext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<CreateGroupPostCommandResult>> Handle(CreateGroupPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _queryContext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var photos = request.Photos ?? new List<PhotoAddOnPost>();
            var videos = request.Videos ?? new List<VideoAddOnPost>();

            Guid PhotoIdSingle = Guid.Empty;
            Guid VideoIdSingle = Guid.Empty;
            int numberPost = photos.Count() + videos.Count();
            bool statusGroup = true;
            var groupSettingName = (from g in _queryContext.GroupFpts
                                    join gsu in _queryContext.GroupSettingUses
                                        on g.GroupId equals gsu.GroupId
                                    join groupSetting in _queryContext.GroupSettings
                                        on gsu.GroupSettingId equals groupSetting.GroupSettingId
                                    join groupStatus in _queryContext.GroupStatuses // Join với bảng GroupStatuses
                                        on gsu.GroupStatusId equals groupStatus.GroupStatusId
                                    where g.GroupId == request.GroupId
                                    select new
                                    {
                                        GroupSettingName = groupSetting.GroupSettingName,
                                        GroupStatusName = groupStatus.GroupStatusName
                                    }).FirstOrDefault();
            if (groupSettingName.GroupSettingName == "Approve Post" && groupSettingName.GroupStatusName == "Public")
            {
                statusGroup = false;
            }

            var groupRoleName = (from gm in _context.GroupMembers
                                 join gr in _context.GroupRoles
                                 on gm.GroupRoleId equals gr.GroupRoleId
                                 where gm.GroupId == request.GroupId && gm.UserId == request.UserId
                                 select gr.GroupRoleName).FirstOrDefault();
            if (groupRoleName == "Admin")
            {
                statusGroup = false;
            }

            Guid groupStatusId = (Guid)_queryContext.GroupFpts.Where(x => x.GroupId == request.GroupId).Select(x => x.GroupStatusId).FirstOrDefault();

            if (photos.Count() == 1 && numberPost == 1)
            {
                PhotoIdSingle = await UploadImage(photos.First().PhotoUrl, request.GroupId);
            }
            if (videos.Count() == 1 && numberPost == 1)
            {
                VideoIdSingle = await UploadVideo(videos.First().VideoUrl, request.GroupId);
            }

            Domain.CommandModels.GroupPost groupPost = new Domain.CommandModels.GroupPost
            {
                GroupPostId = _helper.GenerateNewGuid(),
                UserId = request.UserId,
                Content = request.Content,
                GroupId = request.GroupId,
                GroupPostNumber = DateTime.Now.ToString("ddMMyyHHmmss") + request.UserId.ToString().Replace("-", ""),
                GroupStatusId = groupStatusId,
                IsHide = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                NumberPost = numberPost,
                IsBanned = false,
                IsPending = statusGroup,
            };

            if (PhotoIdSingle != Guid.Empty)
            {
                groupPost.GroupPhotoId = PhotoIdSingle;
            }

            if (VideoIdSingle != Guid.Empty)
            {
                groupPost.GroupVideoId = VideoIdSingle;
            }
            await _context.GroupPosts.AddAsync(groupPost);
            await _context.SaveChangesAsync();
            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(groupPost.Content);
            if (haveBadWord.Any())
            {
                groupPost.IsBanned = true;
                groupPost.Content = _checkContent.MarkBannedWordsInContent(groupPost.Content, haveBadWord);
                await _context.SaveChangesAsync();
            }

            Domain.CommandModels.GroupPostReactCount groupPostReactCount = new Domain.CommandModels.GroupPostReactCount
            {
                GroupPostReactCountId = _helper.GenerateNewGuid(),
                GroupPostId = groupPost.GroupPostId,
                ReactCount = 0,
                CommentCount = 0,
                ShareCount = 0,
            };
            await _context.GroupPostReactCounts.AddAsync(groupPostReactCount);
            await _context.SaveChangesAsync();

            if (numberPost > 1)
            {
                foreach (var photo in photos)
                {
                    Guid photoId = await UploadImage(photo.PhotoUrl, request.GroupId);
                    Domain.CommandModels.GroupPostPhoto groupPostPhoto = new Domain.CommandModels.GroupPostPhoto
                    {
                        GroupPostPhotoId = _helper.GenerateNewGuid(),
                        GroupPostId = groupPost.GroupPostId,
                        GroupPhotoId = photoId,
                        GroupId = groupPost.GroupId,
                        Content = string.Empty,
                        GroupPostPhotoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                        GroupStatusId = groupPost.GroupStatusId,
                        IsHide = groupPost.IsHide,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        PostPosition = photo.Position,
                        IsPending = statusGroup,
                        IsBanned = false,
                    };
                    await _context.GroupPostPhotos.AddAsync(groupPostPhoto);
                    await _context.SaveChangesAsync();

                    Domain.CommandModels.GroupPostReactCount groupPostPhotoReactCount = new Domain.CommandModels.GroupPostReactCount
                    {
                        GroupPostReactCountId = _helper.GenerateNewGuid(),
                        GroupPostPhotoId = groupPostPhoto.GroupPostPhotoId,
                        ReactCount = 0,
                        CommentCount = 0,
                        ShareCount = 0,
                    };
                    await _context.GroupPostReactCounts.AddAsync(groupPostPhotoReactCount);
                    await _context.SaveChangesAsync();
                }

                foreach (var video in videos)
                {
                    Guid videoId = await UploadVideo(video.VideoUrl, request.GroupId);
                    Domain.CommandModels.GroupPostVideo groupPostVideo = new Domain.CommandModels.GroupPostVideo
                    {
                        GroupPostVideoId = _helper.GenerateNewGuid(),
                        GroupPostId = groupPost.GroupPostId,
                        GroupVideoId = videoId,
                        GroupId = groupPost.GroupId,
                        Content = string.Empty,
                        GroupPostVideoNumber = _helper.GenerateNewGuid().ToString().Replace("-", ""),
                        GroupStatusId = groupPost.GroupStatusId,
                        IsHide = groupPost.IsHide,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        PostPosition = video.Position,
                        IsPending = statusGroup,
                        IsBanned = false,
                    };
                    await _context.GroupPostVideos.AddAsync(groupPostVideo);
                    await _context.SaveChangesAsync();

                    Domain.CommandModels.GroupPostReactCount groupPostVideoReactCount = new Domain.CommandModels.GroupPostReactCount
                    {
                        GroupPostReactCountId = _helper.GenerateNewGuid(),
                        GroupPostVideoId = groupPostVideo.GroupPostVideoId,
                        ReactCount = 0,
                        CommentCount = 0,
                        ShareCount = 0,
                    };
                    await _context.GroupPostReactCounts.AddAsync(groupPostVideoReactCount);
                    await _context.SaveChangesAsync();
                }
            }
            var result = _mapper.Map<CreateGroupPostCommandResult>(groupPost);
            result.BannedWords = haveBadWord;
            if(haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.GR09_Group_Post_Have_Bad_Word);
            }
            else
            {
                return Result<CreateGroupPostCommandResult>.Success(result);
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
                UpdatedAt = DateTime.Now
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
                UpdatedAt = DateTime.Now
            };

            await _context.GroupVideos.AddAsync(videoEntity);
            await _context.SaveChangesAsync();

            return videoEntity.GroupVideoId;
        }
    }
}
