using Application.Commands.DeleteGroup;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteGroupPost
{
    public class DeleteGroupPostCommandHandler : ICommandHandler<DeleteGroupPostCommand, DeleteGroupPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteGroupPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<DeleteGroupPostCommandResult>> Handle(DeleteGroupPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null) 
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var groupPost = _querycontext.GroupPosts.Where(x => x.GroupPostId == request.GroupPostId).FirstOrDefault();
            var checkAdmin = await _querycontext.AdminProfiles.Where(x => x.AdminId == request.UserId).Select(y => y.Role.NameRole).FirstOrDefaultAsync();
            bool isAdmin = false;
            if (checkAdmin == "Societe-admin")
            {
                isAdmin = true;
            }
            var result = new DeleteGroupPostCommandResult();
            if (groupPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            else
            {
                if (request.UserId != groupPost.UserId && isAdmin != true)
                {
                    throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
                }
                else
                {
                    var gp = new Domain.CommandModels.GroupPost 
                    {
                        GroupPostId = groupPost.GroupPostId,
                        UserId = groupPost.UserId,
                        Content = groupPost.Content,
                        GroupPostNumber = groupPost.GroupPostNumber,
                        GroupStatusId = groupPost.GroupStatusId,
                        CreatedAt = groupPost.CreatedAt,
                        IsHide = true,
                        UpdatedAt = groupPost.UpdatedAt,
                        GroupPhotoId = groupPost.GroupPhotoId,
                        GroupVideoId = groupPost.GroupVideoId,
                        NumberPost = groupPost.NumberPost,
                        IsBanned = groupPost.IsBanned,
                        GroupId = groupPost.GroupId,
                        IsPending = groupPost.IsPending,
                    };
                    _context.GroupPosts.Update(gp);

                    var GroupPhotoPost = _querycontext.GroupPostPhotos.Where(x => x.GroupPostId == request.GroupPostId).ToList();
                    foreach (var photoPost in GroupPhotoPost) 
                    {
                        var gpp = new Domain.CommandModels.GroupPostPhoto
                        {
                            GroupPostPhotoId = photoPost.GroupPostPhotoId,
                            GroupPostId = photoPost.GroupPostId,
                            Content = photoPost.Content,
                            GroupPhotoId = photoPost.GroupPhotoId,
                            GroupStatusId= photoPost.GroupStatusId,
                            GroupPostPhotoNumber = photoPost.GroupPostPhotoNumber,
                            IsHide= true,
                            CreatedAt = photoPost.CreatedAt,
                            UpdatedAt = photoPost.UpdatedAt,
                            PostPosition = photoPost.PostPosition,
                            IsBanned= photoPost.IsBanned,
                            GroupId = photoPost.GroupId,
                            IsPending= photoPost.IsPending,

                        };
                        _context.GroupPostPhotos.Update(gpp);
                    }
                    var GroupVideoPost = _querycontext.GroupPostVideos.Where(x => x.GroupPostId == request.GroupPostId).ToList();
                    foreach (var videoPost in GroupVideoPost)
                    {
                        var gpp = new Domain.CommandModels.GroupPostVideo
                        {
                            GroupPostVideoId = videoPost.GroupPostVideoId,
                            GroupPostId = videoPost.GroupPostId,
                            Content = videoPost.Content,
                            GroupVideoId = videoPost.GroupPostId,
                            GroupStatusId = videoPost.GroupStatusId,
                            GroupPostVideoNumber = videoPost.GroupPostVideoNumber,
                            IsHide= true,
                            CreatedAt = videoPost.CreatedAt,
                            UpdatedAt = videoPost.UpdatedAt,
                            PostPosition = videoPost.PostPosition,
                            IsBanned = videoPost.IsBanned,
                            GroupId = videoPost.GroupId,
                            IsPending= videoPost.IsPending,
                        };
                        _context.GroupPostVideos.Update(gpp);
                    }
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }
           
            return Result<DeleteGroupPostCommandResult>.Success(result); 
        }
    }
}
