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

namespace Application.Commands.DeleteGroupSharePost
{
    public class DeleteGroupSharePostCommandHandler : ICommandHandler<DeleteGroupSharePostCommand, DeleteGroupSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteGroupSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<DeleteGroupSharePostCommandResult>> Handle(DeleteGroupSharePostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userPost = _querycontext.GroupSharePosts.Where(x => x.GroupSharePostId == request.GroupSharePostId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteUserPostCommandResult();
            if (userPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            else
            {
                if (request.UserId != userPost.UserId)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var gsp = new Domain.CommandModels.GroupSharePost
                    {
                        GroupSharePostId = userPost.GroupSharePostId,
                        UserId = userPost.UserId,
                        Content = userPost.Content,
                        UserPostId = userPost.UserPostId,
                        UserPostVideoId = userPost.UserPostVideoId,
                        UserPostPhotoId = userPost.UserPostPhotoId,
                        GroupPostId = userPost.GroupPostId,
                        GroupPostPhotoId = userPost.GroupPostPhotoId,
                        GroupPostVideoId = userPost.GroupPostVideoId,
                        SharedToUserId = userPost.SharedToUserId,
                        CreateDate = userPost.CreateDate,
                        IsHide = true,
                        GroupStatusId = userPost.GroupStatusId,
                        UpdateDate = userPost.UpdateDate,
                        IsBanned = userPost.IsBanned,
                        GroupId = userPost.GroupId,
                        UserSharedId = userPost.UserSharedId,
                        IsPending = userPost.IsPending,
                    };
                    _context.GroupSharePosts.Update(gsp);
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteGroupSharePostCommandResult>.Success(result);
        }
    }
}
