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

namespace Application.Commands.DeleteGroupPhotoPost
{
    public class DeleteGroupPhotoPostCommandHandler : ICommandHandler<DeleteGroupPhotoPostCommand, DeleteGroupPhotoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteGroupPhotoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
        }

        public async Task<Result<DeleteGroupPhotoPostCommandResult>> Handle(DeleteGroupPhotoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new DeleteGroupPhotoPostCommandResult();
            var PhotoPost = _querycontext.GroupPostPhotos
                                        .Include(x => x.GroupPost)  
                                        .Where(x => x.GroupPostPhotoId == request.GroupPostPhotoId)
                                        .FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            if (PhotoPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            else
            {
                if (request.UserId != PhotoPost.GroupPost.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var gpp = new Domain.CommandModels.GroupPostPhoto
                    {
                        GroupPostPhotoId = PhotoPost.GroupPostPhotoId,
                        GroupPostId = PhotoPost.GroupPostId,
                        Content = PhotoPost.Content,
                        GroupPhotoId = PhotoPost.GroupPhotoId,
                        GroupStatusId = PhotoPost.GroupStatusId,
                        GroupPostPhotoNumber = PhotoPost.GroupPostPhotoNumber,
                        IsHide = true,
                        CreatedAt = PhotoPost.CreatedAt,
                        UpdatedAt = PhotoPost.UpdatedAt,
                        PostPosition = PhotoPost.PostPosition,
                        IsBanned = PhotoPost.IsBanned,
                        GroupId = PhotoPost.GroupId,
                        IsPending = PhotoPost.IsPending,

                    };
                    _context.GroupPostPhotos.Update(gpp);
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }

            return Result<DeleteGroupPhotoPostCommandResult>.Success(result);

        }
    }
}
