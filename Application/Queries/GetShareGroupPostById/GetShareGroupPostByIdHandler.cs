using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Queries.GetGroupPostByGroupId;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Core.Helper;
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

namespace Application.Queries.GetShareGroupPostById
{
    public class GetShareGroupPostByIdHandler : IQueryHandler<GetShareGroupPostByIdQuery, GetShareGroupPostByIdResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetShareGroupPostByIdHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetShareGroupPostByIdResult>> Handle(GetShareGroupPostByIdQuery request, CancellationToken cancellationToken)
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

            var userId = await _context.GroupSharePosts
                .AsNoTracking()
                .Where(x => x.GroupSharePostId == request.GroupSharePostId)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            var groupId = await _context.GroupSharePosts
                .AsNoTracking()
                .Where(x => x.GroupSharePostId == request.GroupSharePostId)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (blockUserList.Contains(userId))
            {
                throw new ErrorException(StatusCodeEnum.UP05_Can_Not_See_Content);
            }

            var isAdmin = await _context.AdminProfiles
                .AnyAsync(x => x.AdminId == request.UserId && x.Role.NameRole == "Societe-admin");

            // Kiểm tra xem user có trong group hay không
            var isJoin = await _context.GroupMembers
                    .AsNoTracking()
                    .AnyAsync(x => x.GroupId == groupId && x.UserId == request.UserId);

            var isPrivate = await _context.GroupSharePosts
                    .AsNoTracking()
                    .Include(x => x.GroupStatus)
                    .AnyAsync(x => x.GroupStatus.GroupStatusName == "Private" && x.GroupSharePostId == request.GroupSharePostId && x.UserId != request.UserId && !isJoin);

            if (!isAdmin && isPrivate)
            {
                throw new ErrorException(StatusCodeEnum.GR16_User_Not_Exist_In_Group);
            }

            var sharePosts = await _context.GroupSharePosts
                .AsNoTracking()
                .Include(x => x.Group)
                .Include(gsp => gsp.GroupStatus)
                .Include(gsp => gsp.GroupPost)
                    .ThenInclude(gp => gp.GroupPhoto)
                .Include(gsp => gsp.GroupPost)
                    .ThenInclude(gp => gp.GroupVideo)
                .Include(gsp => gsp.GroupPost)
                    .ThenInclude(gp => gp.GroupPostPhotos)
                        .ThenInclude(gpp => gpp.GroupPhoto)
                .Include(gsp => gsp.GroupPost)
                    .ThenInclude(gp => gp.GroupPostVideos)
                        .ThenInclude(gpv => gpv.GroupVideo)
                .Include(gp => gp.GroupPostPhoto)
                        .ThenInclude(gpp => gpp.GroupPhoto)
                .Include(gp => gp.GroupPostVideo)
                        .ThenInclude(gpv => gpv.GroupVideo)
                .Include(gsp => gsp.UserPost)
                    .ThenInclude(gp => gp.Photo)
                .Include(gsp => gsp.UserPost)
                    .ThenInclude(gp => gp.Video)
                .Include(gsp => gsp.UserPost)
                    .ThenInclude(up => up.UserPostPhotos)
                        .ThenInclude(upp => upp.Photo)
                .Include(gsp => gsp.UserPost)
                    .ThenInclude(up => up.UserPostVideos)
                        .ThenInclude(upv => upv.Video)
                .Include(gsp => gsp.UserPostPhoto)
                    .ThenInclude(up => up.Photo)
                .Include(gsp => gsp.UserPostVideo)
                    .ThenInclude(upv => upv.Video)
                .Where(gsp =>gsp.GroupSharePostId == request.GroupSharePostId && !blockUserList.Contains(gsp.UserId) && gsp.IsHide != true && gsp.IsBanned != true)
                .FirstOrDefaultAsync(cancellationToken);

            if(sharePosts == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }

            var userProfiles = await _context.UserProfiles
                .AsNoTracking()
                .Where(up => up.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            var avatarPhotos = await _context.AvataPhotos
                .AsNoTracking()
                .Where(ap => ap.UserId == userId && ap.IsUsed)
                .FirstOrDefaultAsync(cancellationToken);

            var userShareProfiles = await _context.UserProfiles
                .AsNoTracking()
                .Where(up => sharePosts.UserSharedId == up.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            var avatarSharePhotos = await _context.AvataPhotos
                .AsNoTracking()
                .Where(ap => sharePosts.UserSharedId == ap.UserId && ap.IsUsed)
                .FirstOrDefaultAsync(cancellationToken);

            var groupIdForShare = await _context.GroupPosts
                .AsNoTracking()
                .Where(gp => sharePosts.GroupPostId == gp.GroupPostId)
                .Select(gp => gp.GroupId)
                .FirstOrDefaultAsync(cancellationToken);

            var groups = await _context.GroupFpts
                .AsNoTracking()
                .Where(g => g.GroupId == groupIdForShare)
                .FirstOrDefaultAsync(cancellationToken);

            var reactGroupShareCounts = await _context.ReactGroupSharePosts
                .AsNoTracking()
                .CountAsync(x => x.GroupSharePostId == request.GroupSharePostId);

            var commentGroupShareCounts = await _context.CommentGroupSharePosts
                .AsNoTracking()
                .CountAsync(c => c.GroupSharePostId == request.GroupSharePostId && c.IsHide != true && c.IsBanned != true);

            var isReact = await _context.ReactGroupSharePosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.GroupSharePostId == sharePosts.GroupSharePostId && x.UserId == request.UserId);

            var topReact = await _context.ReactGroupSharePosts
            .AsNoTracking()
            .Include(x => x.ReactType)
            .Where(x => x.GroupSharePostId == sharePosts.GroupSharePostId)
            .GroupBy(x => x.ReactTypeId)
            .Select(g => new {
                ReactTypeId = g.Key,
                ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                Count = g.Count()
            })
            .OrderByDescending(r => r.Count)
            .Take(2)
            .ToListAsync(cancellationToken);

            var groupSharePost = new GetShareGroupPostByIdResult
            {
                GroupSharePostId = sharePosts.GroupSharePostId,
                UserId = sharePosts.UserId,
                Content = sharePosts.Content,
                UserPostId = sharePosts.UserPostId,
                UserPostPhotoId = sharePosts.UserPostPhotoId,
                UserPostVideoId = sharePosts.UserPostVideoId,
                GroupPostId = sharePosts.GroupPostId,
                GroupPostPhotoId = sharePosts.GroupPostPhotoId,
                GroupPostVideoId = sharePosts.GroupPostVideoId,
                SharedToUserId = sharePosts.SharedToUserId,
                CreateDate = sharePosts.CreateDate,
                UpdateDate = sharePosts.UpdateDate,
                IsHide = sharePosts.IsHide,
                IsBanned = sharePosts.IsBanned,
                IsPending = sharePosts.IsPending,
                GroupPostShare = _mapper.Map<GroupPostDTO>(sharePosts.GroupPost),
                GroupPostPhotoShare = _mapper.Map<GroupPostPhotoDTO>(sharePosts.GroupPostPhoto),
                GroupPostVideoShare = _mapper.Map<GroupPostVideoDTO>(sharePosts.GroupPostVideo),
                UserPostShare = _mapper.Map<UserPostDTO>(sharePosts.UserPost),
                UserPostPhotoShare = _mapper.Map<UserPostPhotoDTO>(sharePosts.UserPostPhoto),
                UserPostVideoShare = _mapper.Map<UserPostVideoDTO>(sharePosts.UserPostVideo),
                UserNameShare = userShareProfiles?.FullName ,
                UserAvatarShare = _mapper.Map<GetUserAvatar>(avatarSharePhotos),
                GroupShareId = groups?.GroupId,
                GroupShareName = groups?.GroupName,
                GroupShareCorverImage = groups?.CoverImage,
                UserName = userProfiles?.FullName,
                UserAvatar = _mapper.Map<GetUserAvatar>(avatarPhotos),
                GroupStatus = new GetGroupStatusDTO
                {
                    GroupStatusId = sharePosts.GroupStatusId ?? Guid.Empty,
                    GroupStatusName = sharePosts.GroupStatus?.GroupStatusName ?? string.Empty,
                },
                GroupId = sharePosts.GroupId,
                GroupName = sharePosts.Group?.GroupName,
                GroupCorverImage = sharePosts.Group?.CoverImage,
                ReactCount = new DTO.ReactDTO.ReactCount
                {
                    ReactNumber = reactGroupShareCounts,
                    CommentNumber = commentGroupShareCounts,
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
                },
            };

            return Result<GetShareGroupPostByIdResult>.Success(groupSharePost);
        }
    }
}
