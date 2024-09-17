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

namespace Application.Commands.DeleteCommentGroupVideoPost
{
    public class DeleteCommentGroupVideoPostCommandHandler : ICommandHandler<DeleteCommentGroupVideoPostCommand, DeleteCommentGroupVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;

        public DeleteCommentGroupVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
        }

        public async Task<Result<DeleteCommentGroupVideoPostCommandResult>> Handle(DeleteCommentGroupVideoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var groupVideoComment = _querycontext.CommentGroupVideoPosts.Where(x => x.CommentGroupVideoPostId == request.CommentGroupVideoPostId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteCommentGroupVideoPostCommandResult();
            if (groupVideoComment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM04_Comment_Not_Found);
            }
            else
            {
                if (request.UserId != groupVideoComment.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var groupPostReactCount = _querycontext.GroupPostReactCounts.Where(x => x.GroupPostVideoId == groupVideoComment.GroupPostVideoId).FirstOrDefault();
                    int totalCommentsDeleted = DeleteCommentAndChildren(groupVideoComment.CommentGroupVideoPostId);

                    var cgvs = new Domain.CommandModels.CommentGroupVideoPost
                    {
                        CommentGroupVideoPostId = groupVideoComment.CommentGroupVideoPostId,
                        GroupPostVideoId = groupVideoComment.GroupPostVideoId,
                        UserId = groupVideoComment.UserId,
                        Content = groupVideoComment.Content,
                        ParentCommentId = groupVideoComment.ParentCommentId,
                        ListNumber = groupVideoComment.ListNumber,
                        LevelCmt = groupVideoComment.LevelCmt,
                        IsHide = true,
                        CreatedDate = groupVideoComment.CreatedDate,
                        IsBanned = groupVideoComment.IsBanned,
                    };
                    _context.CommentGroupVideoPosts.Update(cgvs);
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
                            GroupPostVideoId = groupPostReactCount.GroupPostVideoId,
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

            return Result<DeleteCommentGroupVideoPostCommandResult>.Success(result);
        }

        private int DeleteCommentAndChildren(Guid commentGroupVideoPostId)
        {
            var childComments = _querycontext.CommentGroupVideoPosts
                                .Where(x => x.ParentCommentId == commentGroupVideoPostId)
                                .ToList();

            int countDeleted = 0;

            foreach (var childComment in childComments)
            {
                countDeleted += DeleteCommentAndChildren(childComment.CommentGroupVideoPostId);

                var cgvs = new Domain.CommandModels.CommentGroupVideoPost
                {
                    CommentGroupVideoPostId = childComment.CommentGroupVideoPostId,
                    GroupPostVideoId = childComment.GroupPostVideoId,
                    UserId = childComment.UserId,
                    Content = childComment.Content,
                    ParentCommentId = childComment.ParentCommentId,
                    ListNumber = childComment.ListNumber,
                    LevelCmt = childComment.LevelCmt,
                    IsHide = true,
                    CreatedDate = childComment.CreatedDate,
                    IsBanned = childComment.IsBanned,
                };
                _context.CommentGroupVideoPosts.Update(cgvs);
                countDeleted++;
            }

            return countDeleted;
        }
    }
}
