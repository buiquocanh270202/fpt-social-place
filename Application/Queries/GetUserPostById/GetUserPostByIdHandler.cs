using Application.DTO.GetUserProfileDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserPostById
{
    public class GetUserPostByIdHandler : IQueryHandler<GetUserPostByIdQuery, GetUserPostByIdResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetUserPostByIdHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetUserPostByIdResult>> Handle(GetUserPostByIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var blockUserList = await _context.BlockUsers
                .AsNoTracking()
                .Where(x => (x.UserId == request.UserId || x.UserIsBlockedId == request.UserId) && x.IsBlock == true)
                .Select(x => x.UserId == request.UserId ? x.UserIsBlockedId : x.UserId)
                .ToListAsync(cancellationToken);

            if (blockUserList.Contains((Guid)request.UserId))
            {
                throw new ErrorException(StatusCodeEnum.UP05_Can_Not_See_Content);
            }

            var isAdmin = await _context.AdminProfiles
                .AnyAsync(x => x.AdminId == request.UserId && x.Role.NameRole == "Societe-admin");

            var isPrivate = await _context.UserPosts
                    .Include(x => x.UserStatus)
                    .AnyAsync(x => x.UserStatus.StatusName == "Private" && x.UserPostId == request.UserPostId && x.UserId != request.UserId);

            if (!isAdmin && isPrivate)
            {
                throw new ErrorException(StatusCodeEnum.UP05_Can_Not_See_Content);
            }

            var userId = await _context.UserPosts
                .AsNoTracking()
                .Where(x => x.UserPostId == request.UserPostId)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            var isFriend = await _context.Friends
                    .AnyAsync(x => ((x.UserId == userId && x.FriendId == request.UserId)
                                    || (x.UserId == request.UserId && x.FriendId == userId))
                                    && x.Confirm == true);

            var isFriendStatus = await _context.UserPosts
                    .Include(x => x.UserStatus)
                    .AnyAsync(x => x.UserStatus.StatusName == "Friend" && x.UserPostId == request.UserPostId && x.UserId != request.UserId && !isFriend);

            if (!isAdmin &&  isFriendStatus)
            {
                throw new ErrorException(StatusCodeEnum.UP05_Can_Not_See_Content);
            }

            var userPost = await _context.UserPosts
                .AsNoTracking()
                .Include(x => x.Photo)
                .Include(x => x.Video)
                .Include(x => x.UserPostPhotos.Where(x => x.IsHide != true))
                    .ThenInclude(x => x.Photo)
                .Include(x => x.UserPostVideos.Where(x => x.IsHide != true))
                    .ThenInclude(x => x.Video)
                .Where(x => x.UserPostId == request.UserPostId && x.IsHide != true)
                .FirstOrDefaultAsync(cancellationToken);

            if (userPost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }

            var avt = await _context.AvataPhotos.FirstOrDefaultAsync(x => x.UserId == userPost.UserId && x.IsUsed == true);
            var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userPost.UserId);

            var reactNum = await _context.ReactPosts.CountAsync(x => x.UserPostId == userPost.UserPostId);
            var commentNum = await _context.CommentPosts.CountAsync(x => x.UserPostId == userPost.UserPostId && x.IsHide != true && x.IsBanned != true);
            var shareNum = _context.SharePosts.Count(x => x.UserPostId == userPost.UserPostId) +
                _context.GroupSharePosts.Count(x => x.UserPostId == userPost.UserPostId);

            var isReact = await _context.ReactPosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.UserPostId == request.UserPostId && x.UserId == request.UserId);

            var topReact = await _context.ReactPosts
            .Include(x => x.ReactType)
            .Where(x => x.UserPostId == request.UserPostId)
            .GroupBy(x => x.ReactTypeId)
            .Select(g => new {
                ReactTypeId = g.Key,
                ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                Count = g.Count()
            })
            .OrderByDescending(r => r.Count)
            .Take(2)
            .ToListAsync(cancellationToken);

            var result = new GetUserPostByIdResult
            {
                UserPostId = userPost.UserPostId,
                UserId = userPost.UserId,
                Content = userPost.Content,
                UserPostNumber = userPost.UserPostNumber,
                UserStatus = new GetUserStatusDTO
                {
                    UserStatusId = userPost.UserStatusId,
                    UserStatusName = _context.UserStatuses.Where(x => x.UserStatusId == userPost.UserStatusId).Select(x => x.StatusName).FirstOrDefault()
                },
                IsAvataPost = userPost.IsAvataPost,
                IsCoverPhotoPost = userPost.IsCoverPhotoPost,
                IsHide = userPost.IsHide,
                CreatedAt = userPost.CreatedAt,
                UpdatedAt = userPost.UpdatedAt,
                PhotoId = userPost.PhotoId,
                VideoId = userPost.VideoId,
                NumberPost = userPost.NumberPost,
                Photo = _mapper.Map<PhotoDTO>(userPost.Photo),
                Video = _mapper.Map<VideoDTO>(userPost.Video),
                UserPostPhotos = userPost.UserPostPhotos?.Select(upp => new UserPostPhotoDTO
                {
                    UserPostPhotoId = upp.UserPostPhotoId,
                    UserPostId = upp.UserPostId,
                    PhotoId = upp.PhotoId,
                    Content = upp.Content,
                    UserPostPhotoNumber = upp.UserPostPhotoNumber,
                    UserStatusId = upp.UserStatusId,
                    IsHide = upp.IsHide,
                    CreatedAt = upp.CreatedAt,
                    UpdatedAt = upp.UpdatedAt,
                    PostPosition = upp.PostPosition,
                    Photo = _mapper.Map<PhotoDTO>(upp.Photo),
                }).ToList(),
                UserPostVideos = userPost.UserPostVideos?.Select(upp => new UserPostVideoDTO
                {
                    UserPostVideoId = upp.UserPostVideoId,
                    UserPostId = upp.UserPostId,
                    VideoId = upp.VideoId,
                    Content = upp.Content,
                    UserPostVideoNumber = upp.UserPostVideoNumber,
                    UserStatusId = upp.UserStatusId,
                    IsHide = upp.IsHide,
                    CreatedAt = upp.CreatedAt,
                    UpdatedAt = upp.UpdatedAt,
                    PostPosition = upp.PostPosition,
                    Video = _mapper.Map<VideoDTO>(upp.Video),
                }).ToList(),
                Avatar = _mapper.Map<GetUserAvatar>(avt),
                FullName = user.FirstName + " " + user.LastName,
                ReactCount = new DTO.ReactDTO.ReactCount
                {
                    ReactNumber = reactNum,
                    CommentNumber = commentNum,
                    ShareNumber = shareNum,
                    IsReact = isReact != null ? true : false,
                    UserReactType = isReact == null ? null : new ReactTypeCountDTO
                    {
                        ReactTypeId = isReact.ReactTypeId,
                        ReactTypeName = isReact.ReactType.ReactTypeName,
                        NumberReact = 1
                    },
                    Top2React = topReact.Select(x => new ReactTypeCountDTO
                    {
                        ReactTypeId = x.ReactTypeId,
                        ReactTypeName = x.ReactTypeName,
                        NumberReact = x.Count
                    }).ToList()
                }
            };

            
            return Result<GetUserPostByIdResult>.Success(result);
        }
    }
}
