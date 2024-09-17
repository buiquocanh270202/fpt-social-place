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

namespace Application.Commands.DeleteSharePost
{
    public class DeleteSharePostCommandHandler : ICommandHandler<DeleteSharePostCommand, DeleteSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<DeleteSharePostCommandResult>> Handle(DeleteSharePostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userPost = _querycontext.SharePosts.Where(x => x.SharePostId == request.SharePostId).FirstOrDefault();
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
                if (request.UserId != userPost.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var sp = new Domain.CommandModels.SharePost
                    {
                        SharePostId = userPost.SharePostId,
                        UserId = userPost.UserId,
                        Content = userPost.Content,
                        UserPostId = userPost.UserPostId,
                        UserPostVideoId = userPost.UserPostVideoId,
                        UserPostPhotoId = userPost.UserPostPhotoId,
                        GroupPostId = userPost.GroupPostId,
                        GroupPostPhotoId = userPost.GroupPostPhotoId,
                        GroupPostVideoId = userPost.GroupPostVideoId,
                        SharedToUserId = userPost.SharedToUserId,
                        CreatedDate = userPost.CreatedDate,
                        UserStatusId = userPost.UserStatusId,
                        IsHide = true,
                        UpdateDate = userPost.UpdateDate,
                        IsBanned = userPost.IsBanned,
                        UserSharedId = userPost.UserSharedId,
                    };
                    _context.SharePosts.Update(sp);
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteSharePostCommandResult>.Success(result);
        }
    }
}
