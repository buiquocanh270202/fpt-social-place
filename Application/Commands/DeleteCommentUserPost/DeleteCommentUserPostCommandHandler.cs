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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteCommentUserPost
{
    public class DeleteCommentUserPostCommandHandler : ICommandHandler<DeleteCommentUserPostCommand, DeleteCommentUserPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteCommentUserPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<DeleteCommentUserPostCommandResult>> Handle(DeleteCommentUserPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userComment = _querycontext.CommentPosts.Where(x => x.CommentId == request.CommentId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteCommentUserPostCommandResult();
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
                    var postReactCount = _querycontext.PostReactCounts.Where(x => x.UserPostId == userComment.UserPostId).FirstOrDefault();
                    int totalCommentsDeleted = DeleteCommentAndChildren(userComment.CommentId);

                    Domain.CommandModels.CommentPost commentPost = new Domain.CommandModels.CommentPost
                    {
                        CommentId = userComment.CommentId,
                        UserPostId = userComment.UserPostId,
                        UserId = userComment.UserId,
                        Content = userComment.Content,
                        ParentCommentId = userComment.ParentCommentId,
                        ListNumber = userComment.ListNumber,
                        LevelCmt = userComment.LevelCmt,
                        IsHide = true,
                        CreatedDate = userComment.CreatedDate,
                        IsBanned = userComment.IsBanned,
                    };
                    _context.CommentPosts.Update(commentPost);
                    totalCommentsDeleted += 1;
                    if (postReactCount != null)
                    {
                        if (postReactCount.CommentCount >= totalCommentsDeleted)
                        {
                            postReactCount.CommentCount -= totalCommentsDeleted;
                        }
                        else
                        {
                            postReactCount.CommentCount = 0;
                        }
                        var prc = new Domain.CommandModels.PostReactCount 
                        {
                            PostReactCountId = postReactCount.PostReactCountId,
                            UserPostId = postReactCount.UserPostId,
                            UserPostPhotoId = postReactCount.UserPostPhotoId,
                            ReactCount = postReactCount.ReactCount,
                            CommentCount = postReactCount.CommentCount,
                            ShareCount = postReactCount.ShareCount,
                            CreateAt = postReactCount.CreateAt,
                            UpdateAt = postReactCount.UpdateAt,
                        };
                        _context.PostReactCounts.Update(prc);
                    }
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteCommentUserPostCommandResult>.Success(result);
        }
        private int DeleteCommentAndChildren(Guid commentId)
        {
            var childComments = _querycontext.CommentPosts
                                .Where(x => x.ParentCommentId == commentId)
                                .ToList();

            int countDeleted = 0;

            foreach (var childComment in childComments)
            {
                // Đệ quy xóa các comment con của comment này
                countDeleted += DeleteCommentAndChildren(childComment.CommentId);

                // Đánh dấu xóa comment này
                Domain.CommandModels.CommentPost commentPost = new Domain.CommandModels.CommentPost
                {
                    CommentId = childComment.CommentId,
                    UserPostId = childComment.UserPostId,
                    UserId = childComment.UserId,
                    Content = childComment.Content,
                    ParentCommentId = childComment.ParentCommentId,
                    ListNumber = childComment.ListNumber,
                    LevelCmt = childComment.LevelCmt,
                    IsHide = true,
                    CreatedDate = childComment.CreatedDate,
                    IsBanned = childComment.IsBanned,
                };
                _context.CommentPosts.Update(commentPost);
                countDeleted++;
            }

            return countDeleted;
        }

    }


}
