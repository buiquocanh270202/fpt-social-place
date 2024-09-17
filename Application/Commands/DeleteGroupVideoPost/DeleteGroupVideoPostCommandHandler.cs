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

namespace Application.Commands.DeleteGroupVideoPost
{
    public class DeleteGroupVideoPostCommandHandler : ICommandHandler<DeleteGroupVideoPostCommand, DeleteGroupVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteGroupVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }

        public async Task<Result<DeleteGroupVideoPostCommandResult>> Handle(DeleteGroupVideoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new DeleteGroupVideoPostCommandResult();
            var VideoPost = _querycontext.GroupPostVideos.Include(x => x.GroupPost).Where(x => x.GroupPostVideoId == request.GroupPostVideoId).FirstOrDefault();
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
                if (request.UserId != VideoPost.GroupPost.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var gpp = new Domain.CommandModels.GroupPostVideo
                    {
                        GroupPostVideoId = VideoPost.GroupPostVideoId,
                        GroupPostId = VideoPost.GroupPostId,
                        Content = VideoPost.Content,
                        GroupVideoId = VideoPost.GroupVideoId,
                        GroupStatusId = VideoPost.GroupStatusId,
                        GroupPostVideoNumber = VideoPost.GroupPostVideoNumber,
                        IsHide = true,
                        CreatedAt = VideoPost.CreatedAt,
                        UpdatedAt = VideoPost.UpdatedAt,
                        PostPosition = VideoPost.PostPosition,
                        IsBanned = VideoPost.IsBanned,
                        GroupId = VideoPost.GroupId,
                        IsPending = VideoPost.IsPending,

                    };
                    _context.GroupPostVideos.Update(gpp);
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteGroupVideoPostCommandResult>.Success(result);

        }
    }
}
