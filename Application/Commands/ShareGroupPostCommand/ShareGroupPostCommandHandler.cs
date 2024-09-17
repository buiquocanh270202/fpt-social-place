using Application.Services;
using AutoMapper;
using Core.CQRS.Command;
using Core.CQRS;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Services.CheckingBadWord;
using Core.Helper;
using Microsoft.EntityFrameworkCore.Query;
using System.Net.WebSockets;

namespace Application.Commands.ShareGroupPostCommand
{
    public class ShareGroupPostCommandHandler : ICommandHandler<ShareGroupPostCommand, ShareGroupPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly CheckingBadWord _checkContent;

        public ShareGroupPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<ShareGroupPostCommandResult>> Handle(ShareGroupPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            //var groupPost = await _context.GroupPosts
            //                    .Include(up => up.GroupPostPhotos)
            //                    .Include(up => up.GroupPostVideos)
            //                    .FirstOrDefaultAsync(up => up.GroupPostId == request.GroupPostId, cancellationToken);

            //if (groupPost == null)
            //{
            //    throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found); 
            //}

            //if (groupPost.UserId == request.UserId)
            //{
            //    throw new ErrorException(StatusCodeEnum.UP04_Can_Not_Share_Owner_Post);
            //}

            //if (request.UserPostPhotoId.HasValue && !groupPost.GroupPostPhotos.Any(p => p.GroupPostPhotoId == request.UserPostPhotoId))
            //{
            //    throw new ErrorException(StatusCodeEnum.UP04_Can_Not_Share_Owner_Post);
            //}

            //if (request.UserPostVideoId.HasValue && !groupPost.GroupPostVideos.Any(v => v.GroupPostVideoId == request.UserPostVideoId))
            //{
            //    throw new ErrorException(StatusCodeEnum.UP04_Can_Not_Share_Owner_Post);
            //}
            bool statusGroup = true;
            var groupSettingName = (from g in _querycontext.GroupFpts
                                    join gsu in _querycontext.GroupSettingUses
                                        on g.GroupId equals gsu.GroupId
                                    join groupSetting in _querycontext.GroupSettings
                                        on gsu.GroupSettingId equals groupSetting.GroupSettingId
                                    join groupStatus in _querycontext.GroupStatuses // Join với bảng GroupStatuses
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

            Guid groupStatusId = (Guid)_querycontext.GroupFpts.Where(x => x.GroupId == request.GroupId).Select(x => x.GroupStatusId).FirstOrDefault();

            Domain.CommandModels.GroupSharePost sharePost = new Domain.CommandModels.GroupSharePost
            {
                GroupSharePostId = _helper.GenerateNewGuid(),
                UserId = request.UserId,
                Content = request.Content,
                UserPostId = request.UserPostId,
                UserPostPhotoId = request.UserPostPhotoId,
                UserPostVideoId = request.UserPostVideoId,
                GroupPostId = request.GroupPostId, 
                GroupPostPhotoId = request.GroupPostPhotoId,
                GroupPostVideoId = request.GroupPostVideoId,
                SharedToUserId = request.SharedToUserId, 
                GroupStatusId = groupStatusId,
                CreateDate = DateTime.Now,
                IsHide = false,
                IsBanned = false,
                UserSharedId = request.UserSharedId,
                IsPending = statusGroup,
                GroupId = request.GroupId,
            };

            var countUserPost = _querycontext.PostReactCounts.FirstOrDefault(x =>
                                (x.UserPostId == request.UserPostId && x.UserPostPhotoId == null && x.UserPostVideoId == null) ||
                                (x.UserPostPhotoId == request.UserPostPhotoId && x.UserPostId == null && x.UserPostVideoId == null) ||
                                (x.UserPostVideoId == request.UserPostVideoId && x.UserPostId == null && x.UserPostPhotoId == null)
                                );

            if (countUserPost != null)
            {
                countUserPost.ShareCount++;
                var commandModel = new Domain.CommandModels.PostReactCount
                {
                    PostReactCountId = countUserPost.PostReactCountId,
                    UserPostId = countUserPost.UserPostId,
                    UserPostPhotoId = countUserPost.UserPostPhotoId,
                    ReactCount = countUserPost.ReactCount,
                    CommentCount = countUserPost.CommentCount,
                    ShareCount = countUserPost.ShareCount,
                    CreateAt = countUserPost.CreateAt,
                    UpdateAt = countUserPost.UpdateAt,
                };
                _context.PostReactCounts.Update(commandModel);
                _context.SaveChanges();
            }

            var countGroupPost = _querycontext.GroupPostReactCounts.FirstOrDefault(x =>
                                (x.GroupPostId == request.GroupPostId && x.GroupPostPhotoId == null && x.GroupPostVideoId == null) ||
                                (x.GroupPostPhotoId == request.GroupPostPhotoId && x.GroupPostId == null && x.GroupPostVideoId == null) ||
                                (x.GroupPostVideoId == request.GroupPostVideoId && x.GroupPostId == null && x.GroupPostPhotoId == null)
                                );


            if (countGroupPost != null)
            {
                countGroupPost.ShareCount++;
                var commandModel = new Domain.CommandModels.GroupPostReactCount
                {
                    GroupPostReactCountId = countGroupPost.GroupPostReactCountId,
                    GroupPostId = countGroupPost.GroupPostId,
                    GroupPostPhotoId = countGroupPost.GroupPostPhotoId,
                    ReactCount = countGroupPost.ReactCount,
                    CommentCount = countGroupPost.CommentCount,
                    ShareCount = countGroupPost.ShareCount,
                };
                _context.GroupPostReactCounts.Update(commandModel);
                _context.SaveChanges();
            }

            _context.GroupSharePosts.Add(sharePost);
            _context.SaveChanges();

            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(sharePost.Content);
            if (haveBadWord.Any())
            {
                sharePost.IsBanned = true;
                sharePost.Content = _checkContent.MarkBannedWordsInContent(sharePost.Content, haveBadWord);
                await _context.SaveChangesAsync();
            }

            var result = _mapper.Map<ShareGroupPostCommandResult>(sharePost);
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<ShareGroupPostCommandResult>.Success(result);
            }
        }
    }

}
