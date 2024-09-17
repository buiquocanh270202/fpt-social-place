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

namespace Application.Commands.DeleteUserPost
{
    public class DeleteUserPostCommandHandler : ICommandHandler<DeleteUserPostCommand, DeleteUserPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        public DeleteUserPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<DeleteUserPostCommandResult>> Handle(DeleteUserPostCommand request, CancellationToken cancellationToken)
        {
            if (request == null || _querycontext == null) 
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userPost = _querycontext.UserPosts.Where(x => x.UserPostId == request.UserPostId).FirstOrDefault();
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
                    var up = new Domain.CommandModels.UserPost
                    {
                        UserPostId = userPost.UserPostId,
                        UserId = userPost.UserId,
                        Content = userPost.Content,
                        UserPostNumber = userPost.UserPostNumber,
                        UserStatusId = userPost.UserStatusId,
                        IsAvataPost = userPost.IsAvataPost,
                        IsCoverPhotoPost = userPost.IsCoverPhotoPost,
                        IsHide = true,
                        CreatedAt = userPost.CreatedAt,
                        UpdatedAt = DateTime.Now,
                        PhotoId = userPost.PhotoId,
                        VideoId = userPost.VideoId,
                        NumberPost = userPost.NumberPost,
                        IsBanned = userPost.IsBanned,
                    };
                    _context.UserPosts.Update(up);

                    var userPhotoPost = _querycontext.UserPostPhotos.Where(x => x.UserPostId == request.UserPostId).ToList();
                    foreach (var photoPost in userPhotoPost) 
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
                            CreatedAt= photoPost.CreatedAt,
                            UpdatedAt = photoPost.UpdatedAt,
                            PostPosition = photoPost.PostPosition,
                            IsBanned= photoPost.IsBanned,
                        };
                        _context.UserPostPhotos.Update(pp);
                    }
                    var userVideoPost = _querycontext.UserPostVideos.Where(x => x.UserPostId == request.UserPostId).ToList();
                    foreach (var videoPost in userVideoPost)
                    {
                        videoPost.IsHide = true;
                        var vp = new Domain.CommandModels.UserPostVideo
                        {
                            UserPostVideoId = videoPost.UserPostVideoId,
                            UserPostId = videoPost.UserPostId,
                            VideoId = videoPost.VideoId,
                            Content = videoPost.Content,
                            UserPostVideoNumber = videoPost.UserPostVideoNumber,
                            UserStatusId = videoPost.UserStatusId,
                            IsHide = true,
                            CreatedAt = videoPost.CreatedAt,
                            UpdatedAt = videoPost.UpdatedAt,
                            PostPosition = videoPost.PostPosition,
                            IsBanned = videoPost.IsBanned,
                        };
                        _context.UserPostVideos.Update(vp);
                    }
                    _context.SaveChanges();
                    result.Message = "Delete successfully";
                    result.IsDelete = true;
                }
            }
           
            return Result<DeleteUserPostCommandResult>.Success(result); 
        }
    }
}
