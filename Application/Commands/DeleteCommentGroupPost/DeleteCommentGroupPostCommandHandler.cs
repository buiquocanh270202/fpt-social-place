using Application.Commands.DeleteGroup;
using Application.Commands.DeleteGroupPost;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
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

namespace Application.Commands.DeleteCommentGroupPost
{
    public class DeleteCommentGroupPostCommandHandler : ICommandHandler<DeleteCommentGroupPostCommand, DeleteCommentGroupPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteCommentGroupPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<DeleteCommentGroupPostCommandResult>> Handle(DeleteCommentGroupPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var GroupComment = _querycontext.CommentGroupPosts.Where(x => x.CommentGroupPostId == request.CommentGroupPostId).FirstOrDefault();
            var groupPostReactCount = _querycontext.GroupPostReactCounts.Where(x => x.GroupPostId == GroupComment.GroupPostId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteCommentGroupPostCommandResult();

            if (GroupComment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM04_Comment_Not_Found);
            }
            else
            {
                if (request.UserId != GroupComment.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    int totalCommentsDeleted = DeleteCommentAndChildren(GroupComment.CommentGroupPostId);

                    var cgp = new Domain.CommandModels.CommentGroupPost
                    {
                        CommentGroupPostId = GroupComment.CommentGroupPostId,
                        GroupPostId = GroupComment.GroupPostId,
                        UserId = GroupComment.UserId,
                        Content = GroupComment.Content,
                        ParentCommentId = GroupComment.ParentCommentId,
                        ListNumber = GroupComment.ListNumber,
                        LevelCmt = GroupComment.LevelCmt,
                        IsHide = true,
                        CreatedDate = GroupComment.CreatedDate,
                        IsBanned = GroupComment.IsBanned,
                    };
                    _context.CommentGroupPosts.Update(cgp);
                    totalCommentsDeleted += 1;

                    if (groupPostReactCount != null)
                    {
                        if (groupPostReactCount.CommentCount >= totalCommentsDeleted)
                        {
                            groupPostReactCount.CommentCount -= totalCommentsDeleted;
                        }
                        else
                        {
                            groupPostReactCount.CommentCount = 0;
                        }
                        var prc = new Domain.CommandModels.GroupPostReactCount
                        {
                            GroupPostReactCountId = groupPostReactCount.GroupPostReactCountId,
                            GroupPostId = groupPostReactCount.GroupPostId,
                            GroupPostPhotoId = groupPostReactCount.GroupPostPhotoId,
                            ReactCount = groupPostReactCount.ReactCount,
                            CommentCount = groupPostReactCount.CommentCount,
                            ShareCount = groupPostReactCount.ShareCount,
                        };
                        _context.GroupPostReactCounts.Update(prc);
                    }
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteCommentGroupPostCommandResult>.Success(result);
        }


        private int DeleteCommentAndChildren(Guid commentGroupPostId)
        {
            var childComments = _querycontext.CommentGroupPosts
                                .Where(x => x.ParentCommentId == commentGroupPostId)
                                .ToList();

            int countDeleted = 0;

            foreach (var childComment in childComments)
            {
                countDeleted += DeleteCommentAndChildren(childComment.CommentGroupPostId);

                var cgp = new Domain.CommandModels.CommentGroupPost
                {
                    CommentGroupPostId = childComment.CommentGroupPostId,
                    GroupPostId = childComment.GroupPostId,
                    UserId = childComment.UserId,
                    Content = childComment.Content,
                    ParentCommentId = childComment.ParentCommentId,
                    ListNumber = childComment.ListNumber,
                    LevelCmt = childComment.LevelCmt,
                    IsHide = true,
                    CreatedDate = childComment.CreatedDate,
                    IsBanned = childComment.IsBanned,
                };
                _context.CommentGroupPosts.Update(cgp);
                countDeleted++;
            }

            return countDeleted;
        }

    }
}
