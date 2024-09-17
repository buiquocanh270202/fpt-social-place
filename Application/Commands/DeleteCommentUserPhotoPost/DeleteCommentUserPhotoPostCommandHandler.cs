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

namespace Application.Commands.DeleteCommentUserPhotoPost
{
    public class DeleteCommentUserPhotoPostCommandHandler : ICommandHandler<DeleteCommentUserPhotoPostCommand, DeleteCommentUserPhotoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;

        public DeleteCommentUserPhotoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
        }

        public async Task<Result<DeleteCommentUserPhotoPostCommandResult>> Handle(DeleteCommentUserPhotoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userPhotoComment = _querycontext.CommentPhotoPosts.Where(x => x.CommentPhotoPostId == request.CommentPhotoPostId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteCommentUserPhotoPostCommandResult();
            if (userPhotoComment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM04_Comment_Not_Found);
            }
            else
            {
                var postReactCount = _querycontext.PostReactCounts.Where(x => x.UserPostPhotoId == userPhotoComment.UserPostPhotoId).FirstOrDefault();
                if (request.UserId != userPhotoComment.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    int totalCommentsDeleted = DeleteCommentAndChildren(userPhotoComment.CommentPhotoPostId);

                    Domain.CommandModels.CommentPhotoPost commentPhotoPost = new Domain.CommandModels.CommentPhotoPost
                    {
                        CommentPhotoPostId = userPhotoComment.CommentPhotoPostId,
                        UserPostPhotoId = userPhotoComment.UserPostPhotoId,
                        UserId = userPhotoComment.UserId,
                        Content = userPhotoComment.Content,
                        ParentCommentId = userPhotoComment.ParentCommentId,
                        ListNumber = userPhotoComment.ListNumber,
                        LevelCmt = userPhotoComment.LevelCmt,
                        IsHide = true,
                        CreatedDate = userPhotoComment.CreatedDate,
                        IsBanned = userPhotoComment.IsBanned,
                    };
                    _context.CommentPhotoPosts.Update(commentPhotoPost);
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
                            UpdateAt = DateTime.Now,
                        };

                        _context.PostReactCounts.Update(prc);
                    }

                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteCommentUserPhotoPostCommandResult>.Success(result);
        }

        private int DeleteCommentAndChildren(Guid commentPhotoPostId)
        {
            var childComments = _querycontext.CommentPhotoPosts
                                .Where(x => x.ParentCommentId == commentPhotoPostId)
                                .ToList();

            int countDeleted = 0;

            foreach (var childComment in childComments)
            {
                countDeleted += DeleteCommentAndChildren(childComment.CommentPhotoPostId);

                Domain.CommandModels.CommentPhotoPost commentPhotoPost = new Domain.CommandModels.CommentPhotoPost
                {
                    CommentPhotoPostId = childComment.CommentPhotoPostId,
                    UserPostPhotoId = childComment.UserPostPhotoId,
                    UserId = childComment.UserId,
                    Content = childComment.Content,
                    ParentCommentId = childComment.ParentCommentId,
                    ListNumber = childComment.ListNumber,
                    LevelCmt = childComment.LevelCmt,
                    IsHide = true,
                    CreatedDate = childComment.CreatedDate,
                    IsBanned = childComment.IsBanned,
                };
                _context.CommentPhotoPosts.Update(commentPhotoPost);
                countDeleted++;
            }

            return countDeleted;
        }
    }
}
