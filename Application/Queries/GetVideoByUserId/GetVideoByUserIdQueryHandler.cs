using Application.DTO.CreateUserDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Queries.GetImageByUserId;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetVideoByUserId
{
    public class GetVideoByUserIdQueryHandler : IQueryHandler<GetVideoByUserIdQuery, GetVideoByUserIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetVideoByUserIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetVideoByUserIdQueryResult>> Handle(GetVideoByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new GetVideoByUserIdQueryResult();

            bool isBlock = _context.BlockUsers.Where(x => (request.UserId == x.UserIsBlockedId && request.StrangerId == x.UserId) ||
                                                          (request.UserId == x.UserId && request.StrangerId == x.UserIsBlockedId)).Any();
            if (isBlock)
            {
                return Result<GetVideoByUserIdQueryResult>.Success(result);
            }
            string checkUserStatus = _context.UserSettings.Where(x => x.UserId == request.UserId &&
                                                                    x.Setting.SettingName == "Profile Status").Select(x => x.UserStatus.StatusName).FirstOrDefault();

            if (checkUserStatus != "Public" && (request.UserId != request.StrangerId))
            {
                return Result<GetVideoByUserIdQueryResult>.Success(result);
            }
            string checkRelationship = "Stranger";
            if (request.UserId == request.StrangerId)
            {
                checkRelationship = "Owner";
            }
            else
            {
                bool isFriend = _context.Friends.Any(x => (x.UserId == request.UserId && x.FriendId == request.StrangerId ||
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
            var userVideos = _context.UserPostVideos.Where(x => x.UserPost.UserId == request.StrangerId && x.IsHide != true && x.IsBanned != true
                                                            && (checkRelationship == "Owner" ||
                                                                (checkRelationship == "Friend" && (x.UserPost.UserStatus.StatusName == "Friend" || x.UserPost.UserStatus.StatusName == "Public")) ||
                                                                (checkRelationship == "Stranger" && x.UserPost.UserStatus.StatusName == "Public")))
                                                                .Select(x => new UserVideoDTO
                                                                {
                                                                    UserId = x.UserPost.UserId,
                                                                    UserPostVideoId = x.UserPostVideoId,
                                                                    UserPostId = x.UserPostId,
                                                                    VideoUrl = x.Video.VideoUrl,
                                                                    CreateDate = x.CreatedAt
                                                                })
                                                                .OrderByDescending(x => x.CreateDate)
                                                                .Skip((request.Page - 1) * 5)
                                                                .Take(5)
                                                                .ToList();
            var userVideos2 = _context.UserPosts.Where(x => x.UserId == request.StrangerId && x.IsHide != true && x.IsBanned != true && !string.IsNullOrEmpty(x.VideoId.ToString())
                                                        && (checkRelationship == "Owner" ||
                                                            (checkRelationship == "Friend" && (x.UserStatus.StatusName == "Friend" || x.UserStatus.StatusName == "Public")) ||
                                                            (checkRelationship == "Stranger" && x.UserStatus.StatusName == "Public")))
                                                            .Select(x => new UserVideoDTO
                                                            {
                                                                UserId = x.UserId,
                                                                UserPostVideoId = null,
                                                                UserPostId = x.UserPostId,
                                                                VideoUrl = x.Video.VideoUrl,
                                                                CreateDate = x.CreatedAt
                                                            })
                                                            .OrderByDescending(x => x.CreateDate)
                                                            .Skip((request.Page - 1) * 5)
                                                            .Take(5)
                                                            .ToList();

            userVideos.AddRange(userVideos2);
            userVideos = userVideos.OrderByDescending(x => x.CreateDate).ToList();

            result.Videos = userVideos;

            return Result<GetVideoByUserIdQueryResult>.Success(result);
        }
    }
}
