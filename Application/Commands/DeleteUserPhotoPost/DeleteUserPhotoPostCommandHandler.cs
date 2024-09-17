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

namespace Application.Commands.DeleteUserPhotoPost
{
    public class DeleteUserPhotoPostCommandHandler : ICommandHandler<DeleteUserPhotoPostCommand, DeleteUserPhotoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteUserPhotoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }

        public async Task<Result<DeleteUserPhotoPostCommandResult>> Handle(DeleteUserPhotoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new DeleteUserPhotoPostCommandResult();
            var photoPost = _querycontext.UserPostPhotos.Include(x => x.UserPost).Where(x => x.UserPostPhotoId == request.UserPostPhotoId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            if (photoPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            else
            {
                if (request.UserId != photoPost.UserPost.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var pp = new Domain.CommandModels.UserPostPhoto
                    {
                        UserPostPhotoId = photoPost.UserPostPhotoId,
                        UserPostId = photoPost.UserPostId,
                        PhotoId = photoPost.PhotoId,
                        Content = photoPost.Content,
                        UserPostPhotoNumber = photoPost.UserPostPhotoNumber,
                        UserStatusId = photoPost.UserStatusId,
                        IsHide = true,
                        CreatedAt = photoPost.CreatedAt,
                        UpdatedAt = photoPost.UpdatedAt,
                        PostPosition = photoPost.PostPosition,
                        IsBanned = photoPost.IsBanned,
                    };
                    _context.UserPostPhotos.Update(pp);
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteUserPhotoPostCommandResult>.Success(result);

        }
    }
}
