using Application.Commands.Post;
using Application.Services;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Services.CheckingBadWord;

namespace Application.Commands.ShareUserPostCommand
{
    public class ShareUserPostCommandHandler : ICommandHandler<ShareUserPostCommand, ShareUserPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly CheckingBadWord _checkContent;

        public ShareUserPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<ShareUserPostCommandResult>> Handle(ShareUserPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userPost = await _querycontext.UserPosts
                                .Include(up => up.UserPostPhotos)
                                .Include(up => up.UserPostVideos)
                                .FirstOrDefaultAsync(up => up.UserPostId == request.UserPostId, cancellationToken);

            var groupPost = await _querycontext.GroupPosts
                                .Include(up => up.GroupPostPhotos)
                                .Include(up => up.GroupPostVideos)
                                .FirstOrDefaultAsync(up => up.GroupPostId == request.GroupPostId, cancellationToken);


            if (userPost == null && groupPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found); // Or a more suitable error code
            }

            if (userPost != null && userPost.UserId == request.UserId)
            {
                throw new ErrorException(StatusCodeEnum.UP04_Can_Not_Share_Owner_Post);
            }

            // Additional checks for photo and video ownership (if IDs are provided)
            if (request.UserPostPhotoId.HasValue && !userPost.UserPostPhotos.Any(p => p.UserPostPhotoId == request.UserPostPhotoId))
            {
                throw new ErrorException(StatusCodeEnum.UP04_Can_Not_Share_Owner_Post);
            }

            if (request.UserPostVideoId.HasValue && !userPost.UserPostVideos.Any(v => v.UserPostVideoId == request.UserPostVideoId))
            {
                throw new ErrorException(StatusCodeEnum.UP04_Can_Not_Share_Owner_Post);
            }

            Domain.CommandModels.SharePost sharePost = new Domain.CommandModels.SharePost
            {
                SharePostId = _helper.GenerateNewGuid(),
                UserId = request.UserId,
                Content = request.Content,
                UserPostId = request.UserPostId,
                UserPostVideoId = request.UserPostVideoId,
                UserPostPhotoId = request.UserPostPhotoId,
                GroupPostId = request.GroupPostId,
                GroupPostPhotoId = request.GroupPostPhotoId,
                GroupPostVideoId = request.GroupPostVideoId,
                SharedToUserId = request.SharedToUserId,
                CreatedDate = DateTime.Now,
                UserStatusId = request.UserStatusId,
                IsHide = false,
                IsBanned = false,
                UserSharedId = request.UserSharedId,
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

            _context.SharePosts.Add(sharePost);
            _context.SaveChanges();

            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(sharePost.Content);
            if (haveBadWord.Any())
            {
                sharePost.IsBanned = true;
                sharePost.Content = _checkContent.MarkBannedWordsInContent(sharePost.Content, haveBadWord);
                await _context.SaveChangesAsync();
            }

            var result = _mapper.Map<ShareUserPostCommandResult>(sharePost);
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<ShareUserPostCommandResult>.Success(result);
            }
        }
    }
}
