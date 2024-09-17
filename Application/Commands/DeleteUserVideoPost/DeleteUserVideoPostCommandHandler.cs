using Application.Commands.DeleteGroupPost;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
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

namespace Application.Commands.DeleteUserVideoPost
{
    public class DeleteUserVideoPostCommandHandler : ICommandHandler<DeleteUserVideoPostCommand, DeleteUserVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteUserVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }

        public async Task<Result<DeleteUserVideoPostCommandResult>> Handle(DeleteUserVideoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new DeleteUserVideoPostCommandResult();
            var VideoPost = _querycontext.UserPostVideos.Include(x => x.UserPost).Where(x => x.UserPostVideoId == request.UserPostVideoId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            if (VideoPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            else
            {
                if (request.UserId != VideoPost.UserPost.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var pp = new Domain.CommandModels.UserPostVideo
                    {
                        UserPostVideoId = VideoPost.UserPostVideoId,
                        UserPostId = VideoPost.UserPostId,
                        VideoId = VideoPost.VideoId,
                        Content = VideoPost.Content,
                        UserPostVideoNumber = VideoPost.UserPostVideoNumber,
                        UserStatusId = VideoPost.UserStatusId,
                        IsHide = true,
                        CreatedAt = VideoPost.CreatedAt,
                        UpdatedAt = VideoPost.UpdatedAt,
                        PostPosition = VideoPost.PostPosition,
                        IsBanned = VideoPost.IsBanned,
                    };
                    _context.UserPostVideos.Update(pp);
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteUserVideoPostCommandResult>.Success(result);

        }
    }
}
