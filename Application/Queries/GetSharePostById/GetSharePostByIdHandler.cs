using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Queries.GetPost;
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

namespace Application.Queries.GetSharePostById
{
    public class GetSharePostByIdHandler : IQueryHandler<GetSharePostByIdQuery, GetSharePostByIdResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetSharePostByIdHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetSharePostByIdResult>> Handle(GetSharePostByIdQuery request, CancellationToken cancellationToken)
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

            var userId = await _context.SharePosts
                .AsNoTracking()
                .Where(x => x.SharePostId == request.SharePostId)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (blockUserList.Contains(userId))
            {
                throw new ErrorException(StatusCodeEnum.UP05_Can_Not_See_Content);
            }

            var isPrivate = await _context.SharePosts
                    .AsNoTracking()
                    .Include(x => x.UserStatus)
                    .AnyAsync(x => x.UserStatus.StatusName == "Private" && x.SharePostId == request.SharePostId && x.UserId != request.UserId);

            var isAdmin = await _context.AdminProfiles
                .AnyAsync(x => x.AdminId == request.UserId && x.Role.NameRole == "Societe-admin");

            if (!isAdmin && isPrivate)
            {
                throw new ErrorException(StatusCodeEnum.UP05_Can_Not_See_Content);
            }

            var isFriend = await _context.Friends
                    .AnyAsync(x => ((x.UserId == userId && x.FriendId == request.UserId)
                                    || (x.UserId == request.UserId && x.FriendId == userId))
                                    && x.Confirm == true);

            var isFriendStatus = await _context.SharePosts
                    .Include(x => x.UserStatus)
                    .AnyAsync(x => x.UserStatus.StatusName == "Friend" && x.SharePostId == request.SharePostId && x.UserId != request.UserId && !isFriend);

            if (!isAdmin && isFriendStatus)
            {
                throw new ErrorException(StatusCodeEnum.UP05_Can_Not_See_Content);
            }

            var sharePosts = await _context.SharePosts
                .AsNoTracking()
                .Include(x => x.UserStatus)
                .Include(x => x.UserPost)
                    .ThenInclude(x => x.Photo)
                .Include(x => x.UserPost)
                    .ThenInclude(x => x.Video)
                .Include(x => x.UserPost)
                    .ThenInclude(x => x.UserPostPhotos)
                        .ThenInclude(x => x.Photo)
                .Include(x => x.UserPost)
                    .ThenInclude(x => x.UserPostVideos)
                        .ThenInclude(x => x.Video)
                .Include(x => x.UserPostPhoto)
                    .ThenInclude(x => x.Photo)
                .Include(x => x.UserPostVideo)
                    .ThenInclude(x => x.Video)
                .Include(x => x.GroupPost)
                    .ThenInclude(x => x.GroupPhoto)
                .Include(x => x.GroupPost)
                    .ThenInclude(x => x.GroupVideo)
                .Include(x => x.GroupPost)
                    .ThenInclude(x => x.GroupPostPhotos)
                        .ThenInclude(x => x.GroupPhoto)
                .Include(x => x.GroupPost)
                    .ThenInclude(x => x.GroupPostVideos)
                        .ThenInclude(x => x.GroupVideo)
                .Include(x => x.GroupPostPhoto)
                    .ThenInclude(x => x.GroupPhoto)
                .Include(x => x.GroupPostVideo)
                    .ThenInclude(x => x.GroupVideo)
                .Where(p => p.SharePostId == request.SharePostId && p.IsHide != true && p.IsBanned != true)
                .FirstOrDefaultAsync(cancellationToken);

            if(sharePosts == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }

            var user = _context.UserProfiles.Where(x => x.UserId == sharePosts.UserId)
                                                .Select(x => x.FirstName + " " + x.LastName)
                                                .FirstOrDefault();

            var avatar = _context.AvataPhotos.FirstOrDefault(x => x.UserId == sharePosts.UserId && x.IsUsed == true);

            var userShare = _context.UserProfiles
                .Where(x => x.UserId == sharePosts.UserSharedId)
                .Select(x => x.FirstName + " " + x.LastName)
                .FirstOrDefault();

            var avtShare = _context.AvataPhotos.FirstOrDefault(x => x.UserId == sharePosts.UserSharedId && x.IsUsed == true);

            var groupId = _context.GroupPosts.Where(x => x.GroupPostId == sharePosts.GroupPostId).Select(x => x.GroupId).FirstOrDefault();
            var group = _context.GroupFpts.FirstOrDefault(x => x.GroupId == groupId);

            var reactNumber = _context.ReactSharePosts.Count(x => x.SharePostId == sharePosts.SharePostId);
            var commentNumber = _context.CommentSharePosts
                .Count(x => x.SharePostId == sharePosts.SharePostId && x.IsHide != true && x.IsBanned != true);

            var isReact = await _context.ReactSharePosts
                .Include(x => x.ReactType)
                .FirstOrDefaultAsync(x => x.SharePostId == sharePosts.SharePostId && x.UserId == request.UserId);

            var topReact = await _context.ReactSharePosts
            .Include(x => x.ReactType)
            .Where(x => x.SharePostId == sharePosts.SharePostId)
            .GroupBy(x => x.ReactTypeId)
            .Select(g => new {
                ReactTypeId = g.Key,
                ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                Count = g.Count()
            })
            .OrderByDescending(r => r.Count)
            .Take(2)
            .ToListAsync(cancellationToken);

            var post = new GetSharePostByIdResult
            {
                SharePostId = sharePosts.SharePostId,
                UserId = sharePosts.UserId,
                Content = sharePosts.Content,
                UserPostId = sharePosts.UserPostId,
                UserPostPhotoId = sharePosts.UserPostPhotoId,
                UserPostVideoId = sharePosts.UserPostVideoId,
                GroupPostId = sharePosts.GroupPostId,
                GroupPostPhotoId = sharePosts.GroupPostPhotoId,
                GroupPostVideoId = sharePosts.GroupPostVideoId,
                //SharedToUserId = sharePosts.SharedToUserId,
                CreatedDate = sharePosts.CreatedDate,
                UpdateDate = sharePosts.UpdateDate,
                IsHide = sharePosts.IsHide,
                IsBanned = sharePosts.IsBanned,
                GroupPostShare = _mapper.Map<GroupPostDTO>(sharePosts.GroupPost),
                GroupPostPhotoShare = _mapper.Map<GroupPostPhotoDTO>(sharePosts.GroupPostPhoto),
                GroupPostVideoShare = _mapper.Map<GroupPostVideoDTO>(sharePosts.GroupPostVideo),
                UserPostShare = _mapper.Map<UserPostDTO>(sharePosts.UserPost),
                UserPostPhotoShare = _mapper.Map<UserPostPhotoDTO>(sharePosts.UserPostPhoto),
                UserPostVideoShare = _mapper.Map<UserPostVideoDTO>(sharePosts.UserPostVideo),
                UserNameShare = userShare,
                UserAvatarShare = _mapper.Map<GetUserAvatar>(avtShare),
                GroupShareId = group?.GroupId ?? null,
                GroupShareName = group?.GroupName ?? null,
                GroupShareCorverImage = group?.CoverImage ?? null,
                UserName = user,
                UserAvatar = _mapper.Map<GetUserAvatar>(avatar),
                UserStatus = new DTO.GetUserProfileDTO.GetUserStatusDTO
                {
                    UserStatusId = sharePosts.UserStatusId,
                    UserStatusName = sharePosts.UserStatus.StatusName
                },
                ReactCount = new DTO.ReactDTO.ReactCount
                {
                    ReactNumber = reactNumber,
                    CommentNumber = commentNumber,
                    ShareNumber = 0,
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

            return Result<GetSharePostByIdResult>.Success(post);
        }
    }
}
