using Application.Commands.DeleteGroup;
using Application.Commands.DeleteUserPost;
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
using System.Linq;
using System.Threading.Tasks;

namespace Application.Commands.DeleteCommentGroupSharePost
{
    public class DeleteCommentGroupSharePostCommandHandler : ICommandHandler<DeleteCommentGroupSharePostCommand, DeleteCommentGroupSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;

        public DeleteCommentGroupSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
        }

        public async Task<Result<DeleteCommentGroupSharePostCommandResult>> Handle(DeleteCommentGroupSharePostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userComment = _querycontext.CommentGroupSharePosts.Where(x => x.CommentGroupSharePostId == request.CommentGroupSharePostId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteCommentGroupSharePostCommandResult();

            if (userComment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM04_Comment_Not_Found);
            }
            else
            {
                if (request.UserId != userComment.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    int totalCommentsDeleted = DeleteCommentAndChildren(userComment.CommentGroupSharePostId);

                    Domain.CommandModels.CommentGroupSharePost cgsp = new Domain.CommandModels.CommentGroupSharePost
                    {
                        CommentGroupSharePostId = userComment.CommentGroupSharePostId,
                        GroupSharePostId = userComment.GroupSharePostId,
                        UserId = userComment.UserId,
                        Content = userComment.Content,
                        ParentCommentId = userComment.ParentCommentId,
                        ListNumber = userComment.ListNumber,
                        LevelCmt = userComment.LevelCmt,
                        IsHide = true,
                        CreateDate = userComment.CreateDate,
                        IsBanned = userComment.IsBanned,
                    };
                    _context.CommentGroupSharePosts.Update(cgsp);
                    totalCommentsDeleted += 1;

                    // Optionally: Update Comment Count in GroupSharePost

                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteCommentGroupSharePostCommandResult>.Success(result);
        }

        private int DeleteCommentAndChildren(Guid commentId)
        {
            var childComments = _querycontext.CommentGroupSharePosts
                .Where(x => x.ParentCommentId == commentId)
                .ToList();

            int countDeleted = 0;

            foreach (var childComment in childComments)
            {
                countDeleted += DeleteCommentAndChildren(childComment.CommentGroupSharePostId);

                Domain.CommandModels.CommentGroupSharePost cgsp = new Domain.CommandModels.CommentGroupSharePost
                {
                    CommentGroupSharePostId = childComment.CommentGroupSharePostId,
                    GroupSharePostId = childComment.GroupSharePostId,
                    UserId = childComment.UserId,
                    Content = childComment.Content,
                    ParentCommentId = childComment.ParentCommentId,
                    ListNumber = childComment.ListNumber,
                    LevelCmt = childComment.LevelCmt,
                    IsHide = true,
                    CreateDate = childComment.CreateDate,
                    IsBanned = childComment.IsBanned,
                };
                _context.CommentGroupSharePosts.Update(cgsp);
                countDeleted++;
            }

            return countDeleted;
        }
    }
}
