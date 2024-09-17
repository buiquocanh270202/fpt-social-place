using Application.DTO.NotificationDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.Queries.GetListFriendToInvate;
using Application.Queries.GetUserStatus;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.GetImageByUserId
{
    public class GetImageByUserIdQueryHandler : IQueryHandler<GetImageByUserIdQuery, GetImageByUserIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetImageByUserIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetImageByUserIdQueryResult>> Handle(GetImageByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new GetImageByUserIdQueryResult();
            bool isBlock = _context.BlockUsers.Where(x => (request.UserId == x.UserIsBlockedId && request.StrangerId == x.UserId) ||
                                                          (request.UserId == x.UserId && request.StrangerId == x.UserIsBlockedId)).Any();
            if (isBlock) 
            {
                return Result<GetImageByUserIdQueryResult>.Success(result);
            }
            string checkUserStatus = _context.UserSettings.Where(x => x.UserId == request.UserId && 
                                                                    x.Setting.SettingName == "Profile Status").Select(x => x.UserStatus.StatusName).FirstOrDefault();

            if (checkUserStatus != "Public" && (request.UserId != request.StrangerId))
            {
                return Result<GetImageByUserIdQueryResult>.Success(result);
            }
            string checkRelationship = "Stranger";
            if (request.UserId == request.StrangerId)
            {
                checkRelationship = "Owner";
            }
            else
            {
                bool isFriend = _context.Friends.Any(x =>(x.UserId == request.UserId && x.FriendId == request.StrangerId ||
                                                          x.UserId == request.StrangerId && x.FriendId == request.UserId)
                                                         && x.Confirm == true);
                if (isFriend) 
                {
                    checkRelationship = "Friend";
                }
                else
                {
                    checkRelationship = "Stranger";
                }
            }

            // Tạo list chứa các UserPostPhoto
            List<UserPhotoDTO> userPhotos = new List<UserPhotoDTO>();

            if (request.Type == "Avata")
            {
                userPhotos = GetImageAvatar(request.StrangerId, checkRelationship, request.Page);
            }
            else if (request.Type == "Cover")
            {
                userPhotos = GetImageCover(request.StrangerId, checkRelationship, request.Page);
            }
            else if (request.Type == "Post")
            {
                userPhotos = GetImagePost(request.StrangerId, checkRelationship, request.Page);
            }
            else
            {
                var avatarPhotos = GetImageAvatar(request.StrangerId, checkRelationship, request.Page);
                var coverPhotos = GetImageCover(request.StrangerId, checkRelationship, request.Page);
                var postPhotos = GetImagePost(request.StrangerId, checkRelationship, request.Page);

                userPhotos.AddRange(avatarPhotos);
                userPhotos.AddRange(coverPhotos);
                userPhotos.AddRange(postPhotos);
                userPhotos = userPhotos.OrderByDescending(x => x.CreateDate).ToList();
            }

            result.Photos = userPhotos;

            return Result<GetImageByUserIdQueryResult>.Success(result);
        }

        private List<UserPhotoDTO> GetImageAvatar(Guid userId, string relationshipStatus, int Page)
        {
            var avatarPhotos = _context.UserPosts
                .Where(x => x.UserId == userId && x.IsAvataPost == true && x.IsHide != true && x.IsBanned != true
                            && (relationshipStatus == "Owner" ||
                                (relationshipStatus == "Friend" && (x.UserStatus.StatusName == "Friend" || x.UserStatus.StatusName == "Public")) ||
                                (relationshipStatus == "Stranger" && x.UserStatus.StatusName == "Public")))
                .Select(x => new UserPhotoDTO
                {
                    UserId = x.UserId,
                    UserPostPhotoId = null,
                    UserPostId = x.UserPostId,
                    PhotoUrl = x.Photo.PhotoUrl,
                    CreateDate = x.CreatedAt
                })
                .OrderByDescending(x => x.CreateDate)
                .Skip((Page - 1) * 10)
                .Take(10)
                .ToList();
            return avatarPhotos;
        }

        private List<UserPhotoDTO> GetImageCover(Guid userId, string relationshipStatus, int Page)
        {
            var coverPhotos = _context.UserPosts
                .Where(x => x.UserId == userId && x.IsCoverPhotoPost == true && x.IsHide != true && x.IsBanned != true
                            && (relationshipStatus == "Owner" ||
                                (relationshipStatus == "Friend" && (x.UserStatus.StatusName == "Friend" || x.UserStatus.StatusName == "Public")) ||
                                (relationshipStatus == "Stranger" && x.UserStatus.StatusName == "Public")))
                .Select(x => new UserPhotoDTO
                {
                    UserId = x.UserId,
                    UserPostPhotoId = null,
                    UserPostId = x.UserPostId,
                    PhotoUrl = x.Photo.PhotoUrl,
                    CreateDate = x.CreatedAt
                })
                .OrderByDescending(x => x.CreateDate)
                .Skip((Page - 1) * 10)
                .Take(10)
                .ToList();
            return coverPhotos;
        }

        private List<UserPhotoDTO> GetImagePost(Guid userId, string relationshipStatus, int Page)
        {
            var postPhotos = _context.UserPostPhotos
                .Where(x => x.UserPost.UserId == userId && x.IsHide != true && x.IsBanned != true 
                            && (relationshipStatus == "Owner" ||
                                (relationshipStatus == "Friend" && (x.UserPost.UserStatus.StatusName == "Friend" || x.UserPost.UserStatus.StatusName == "Public")) ||
                                (relationshipStatus == "Stranger" && x.UserPost.UserStatus.StatusName == "Public")))
                .Select(x => new UserPhotoDTO
                {
                    UserId = x.UserPost.UserId,
                    UserPostPhotoId = x.UserPostPhotoId,
                    UserPostId = x.UserPostId,
                    PhotoUrl = x.Photo.PhotoUrl,
                    CreateDate = x.CreatedAt
                })
                .OrderByDescending(x => x.CreateDate)
                .Skip((Page - 1) * 5)
                .Take(5)
                .ToList();

            var postPhotos2 = _context.UserPosts
                .Where(x => x.UserId == userId && x.IsHide != true && x.IsBanned != true && !string.IsNullOrEmpty(x.PhotoId.ToString()) && x.IsAvataPost != true && x.IsCoverPhotoPost != true
                            && (relationshipStatus == "Owner" ||
                                (relationshipStatus == "Friend" && (x.UserStatus.StatusName == "Friend" || x.UserStatus.StatusName == "Public")) ||
                                (relationshipStatus == "Stranger" && x.UserStatus.StatusName == "Public")))
                .Select(x => new UserPhotoDTO
                {
                    UserId = x.UserId,
                    UserPostPhotoId = null,
                    UserPostId = x.UserPostId,
                    PhotoUrl = x.Photo.PhotoUrl,
                    CreateDate = x.CreatedAt
                })
                .OrderByDescending(x => x.CreateDate)
                .Skip((Page - 1) * 5)
                .Take(5)
                .ToList();

            postPhotos.AddRange(postPhotos2);
            return postPhotos;
        }

    }
}
