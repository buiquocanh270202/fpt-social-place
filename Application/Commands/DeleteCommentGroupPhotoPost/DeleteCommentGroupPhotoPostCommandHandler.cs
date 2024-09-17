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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteCommentGroupPhotoPost
{
    public class DeleteCommentGroupPhotoPostCommandHandler : ICommandHandler<DeleteCommentGroupPhotoPostCommand, DeleteCommentGroupPhotoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteCommentGroupPhotoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<DeleteCommentGroupPhotoPostCommandResult>> Handle(DeleteCommentGroupPhotoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var GroupPhotoComment = _querycontext.CommentPhotoGroupPosts.Where(x => x.CommentPhotoGroupPostId == request.CommentPhotoGroupPostId).FirstOrDefault();
            var groupPostReactCount = _querycontext.GroupPostReactCounts.Where(x => x.GroupPostPhotoId == GroupPhotoComment.GroupPostPhotoId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteCommentGroupPhotoPostCommandResult();

            if (GroupPhotoComment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM04_Comment_Not_Found);
            }
            else
            {
                if (request.UserId != GroupPhotoComment.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    int totalCommentsDeleted = DeleteCommentAndChildren(GroupPhotoComment.CommentPhotoGroupPostId);

                    var cpgs = new Domain.CommandModels.CommentPhotoGroupPost
                    {
                        CommentPhotoGroupPostId = GroupPhotoComment.CommentPhotoGroupPostId,
                        GroupPostPhotoId = GroupPhotoComment.GroupPostPhotoId,
                        UserId = GroupPhotoComment.UserId,
                        Content = GroupPhotoComment.Content,
                        ParentCommentId = GroupPhotoComment.ParentCommentId,
                        ListNumber = GroupPhotoComment.ListNumber,
                        LevelCmt = GroupPhotoComment.LevelCmt,
                        IsHide = true,
                        CreatedDate = GroupPhotoComment.CreatedDate,
                        IsBanned = GroupPhotoComment.IsBanned,
                    };
                    _context.CommentPhotoGroupPosts.Update(cpgs);
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

            return Result<DeleteCommentGroupPhotoPostCommandResult>.Success(result);
        }

        private int DeleteCommentAndChildren(Guid commentPhotoGroupPostId)
        {
            var childComments = _querycontext.CommentPhotoGroupPosts
                                .Where(x => x.ParentCommentId == commentPhotoGroupPostId)
                                .ToList();

            int countDeleted = 0;

            foreach (var childComment in childComments)
            {
                countDeleted += DeleteCommentAndChildren(childComment.CommentPhotoGroupPostId);

                var cpgs = new Domain.CommandModels.CommentPhotoGroupPost
                {
                    CommentPhotoGroupPostId = childComment.CommentPhotoGroupPostId,
                    GroupPostPhotoId = childComment.GroupPostPhotoId,
                    UserId = childComment.UserId,
                    Content = childComment.Content,
                    ParentCommentId = childComment.ParentCommentId,
                    ListNumber = childComment.ListNumber,
                    LevelCmt = childComment.LevelCmt,
                    IsHide = true,
                    CreatedDate = childComment.CreatedDate,
                    IsBanned = childComment.IsBanned,
                };
                _context.CommentPhotoGroupPosts.Update(cpgs);
                countDeleted++;
            }

            return countDeleted;
        }

    }
}
