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

namespace Application.Commands.DeleteCommentUserVideoPost
{
    public class DeleteCommentUserVideoPostCommandHandler : ICommandHandler<DeleteCommentUserVideoPostCommand, DeleteCommentUserVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;

        public DeleteCommentUserVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
        }

        public async Task<Result<DeleteCommentUserVideoPostCommandResult>> Handle(DeleteCommentUserVideoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userVideoComment = _querycontext.CommentVideoPosts.Where(x => x.CommentVideoPostId == request.CommentVideoPostId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteCommentUserVideoPostCommandResult();
            if (userVideoComment == null && isAdmin != true)
            {
                throw new ErrorException(StatusCodeEnum.CM04_Comment_Not_Found);
            }
            else
            {
                if (request.UserId != userVideoComment.UserId)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var postReactCount = _querycontext.PostReactCounts.Where(x => x.UserPostVideoId == userVideoComment.UserPostVideoId).FirstOrDefault();
                    int totalCommentsDeleted = DeleteCommentAndChildren(userVideoComment.CommentVideoPostId);

                    Domain.CommandModels.CommentVideoPost commentVideoPost = new Domain.CommandModels.CommentVideoPost
                    {
                        CommentVideoPostId = userVideoComment.CommentVideoPostId,
                        UserPostVideoId = userVideoComment.UserPostVideoId,
                        UserId = userVideoComment.UserId,
                        Content = userVideoComment.Content,
                        ParentCommentId = userVideoComment.ParentCommentId,
                        ListNumber = userVideoComment.ListNumber,
                        LevelCmt = userVideoComment.LevelCmt,
                        IsHide = true,
                        CreatedDate = userVideoComment.CreatedDate,
                        IsBanned = userVideoComment.IsBanned,
                    };
                    _context.CommentVideoPosts.Update(commentVideoPost);
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
                            UserPostVideoId = postReactCount.UserPostVideoId,
                            ReactCount = postReactCount.ReactCount,
                            CommentCount = postReactCount.CommentCount,
                            ShareCount = postReactCount.ShareCount,
                            CreateAt = postReactCount.CreateAt,
                            UpdateAt = DateTime.Now,
                        };

                        _context.PostReactCounts.Update(prc);
                    }

                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteCommentUserVideoPostCommandResult>.Success(result);
        }

        private int DeleteCommentAndChildren(Guid commentVideoPostId)
        {
            var childComments = _querycontext.CommentVideoPosts
                                .Where(x => x.ParentCommentId == commentVideoPostId)
                                .ToList();

            int countDeleted = 0;

            foreach (var childComment in childComments)
            {
                countDeleted += DeleteCommentAndChildren(childComment.CommentVideoPostId);

                Domain.CommandModels.CommentVideoPost commentVideoPost = new Domain.CommandModels.CommentVideoPost
                {
                    CommentVideoPostId = childComment.CommentVideoPostId,
                    UserPostVideoId = childComment.UserPostVideoId,
                    UserId = childComment.UserId,
                    Content = childComment.Content,
                    ParentCommentId = childComment.ParentCommentId,
                    ListNumber = childComment.ListNumber,
                    LevelCmt = childComment.LevelCmt,
                    IsHide = true,
                    CreatedDate = childComment.CreatedDate,
                    IsBanned = childComment.IsBanned,
                };
                _context.CommentVideoPosts.Update(commentVideoPost);
                countDeleted++;
            }

            return countDeleted;
        }
    }
}
