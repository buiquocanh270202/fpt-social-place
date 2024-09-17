using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Queries.GetPost;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupPostByGroupId
{
    public class GetGroupPostByGroupIdHandler : IQueryHandler<GetGroupPostByGroupIdQuery, GetGroupPostByGroupIdResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetGroupPostByGroupIdHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private static List<GetGroupPostByGroupIdDTO> ApplySortingAndPaging(List<GetGroupPostByGroupIdDTO> query, string type, int page, int pageSize)
        {
            if (type.Contains("Valid"))
            {
                query = query.OrderByDescending(x => x.EdgeRank)
                             .ThenByDescending(x => x.CreatedAt)
                             .ToList();
            }
            else if (type.Contains("New"))
            {
                query = query.OrderByDescending(x => x.CreatedAt)
                    .ToList();
            }

            return query.Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public async Task<Result<GetGroupPostByGroupIdResult>> Handle(GetGroupPostByGroupIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var isDelete = await _context.GroupFpts.AsNoTracking().AnyAsync(x => x.GroupId == request.GroupId && x.IsDelete == true);
            if (isDelete)
            {
                throw new ErrorException(StatusCodeEnum.GR17_Group_Is_Not_Exist);
            }
            
            // Lấy ra listuser mà user chặn hoặc bị chặn
            var blockUserList = await _context.BlockUsers
                .AsNoTracking()
                .Where(x => (x.UserId == request.UserId || x.UserIsBlockedId == request.UserId) && x.IsBlock == true)
                .Select(x => x.UserId == request.UserId ? x.UserIsBlockedId : x.UserId)
                .ToListAsync(cancellationToken);

            var isGroupAdmin = await _context.GroupMembers
                .AsNoTracking()
                .Include(x => x.GroupRole)
                .AnyAsync(x => x.GroupRole.GroupRoleName == "Admin" || x.GroupRole.GroupRoleName == "Censor");

            var isAdmin = await _context.AdminProfiles
                .AnyAsync(x => x.AdminId == request.UserId && x.Role.NameRole == "Societe-admin");

            if (isGroupAdmin || isAdmin)
            {
                blockUserList = new List<Guid>();
            }

            var isPublic = await _context.GroupSettingUses
                .AsNoTracking()
                .Include(x => x.GroupStatus)
                .AnyAsync(x => x.GroupId == request.GroupId  && x.GroupStatus.GroupStatusName == "Public");

            var isJoin = await _context.GroupMembers
                .AsNoTracking()
                .AnyAsync(x => x.GroupId == request.GroupId && x.UserId == request.UserId && x.IsJoined == true);

            // Tạo danh sách kết quả
            var combine = new List<GetGroupPostByGroupIdDTO>();
            if (!isJoin && !isPublic && !isAdmin)
            {
                var getgroupposts = new GetGroupPostByGroupIdResult();

                getgroupposts.totalPage = 0;
                getgroupposts.result = combine;
                return Result<GetGroupPostByGroupIdResult>.Success(getgroupposts);
            }

            // Truy vấn các bài đăng nhóm với các thông tin liên quan
            var groupPosts = await _context.GroupPosts
                .AsNoTracking()
                .Include(gp => gp.Group)
                .Include(gp => gp.GroupStatus)
                .Include(gp => gp.GroupPhoto)
                .Include(gp => gp.GroupVideo)
                .Include(gp => gp.GroupPostPhotos.Where(x => x.IsHide != true && x.IsBanned != true))
                    .ThenInclude(gpp => gpp.GroupPhoto)
                .Include(gp => gp.GroupPostVideos.Where(x => x.IsHide != true && x.IsBanned != true))
                    .ThenInclude(gpv => gpv.GroupVideo)
                .Where(gp => gp.GroupId == request.GroupId && !blockUserList.Contains(gp.UserId) && gp.IsHide != true && gp.IsBanned != true && gp.IsPending == false)
                .ToListAsync(cancellationToken);

            var groupPostIds = groupPosts.Select(gp => gp.GroupPostId).ToList();

            // Truy vấn thông tin người dùng, ảnh đại diện và số lượng tương tác một lần cho tất cả bài đăng
            var userProfiles = await _context.UserProfiles
                .AsNoTracking()
                .Where(up => groupPosts.Select(gp => gp.UserId).Contains(up.UserId) && !blockUserList.Contains(up.UserId))
                .ToListAsync(cancellationToken);

            var avatarPhotos = await _context.AvataPhotos
                .AsNoTracking()
                .Where(ap => groupPosts.Select(gp => gp.UserId).Contains(ap.UserId) && !blockUserList.Contains(ap.UserId) && ap.IsUsed)
                .ToListAsync(cancellationToken);

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
                .Where(gsp => gsp.GroupId == request.GroupId && !blockUserList.Contains(gsp.UserId) && gsp.IsHide != true && gsp.IsBanned != true && gsp.IsPending == false)
                .ToListAsync(cancellationToken);

            var userShareProfiles = await _context.UserProfiles
                .AsNoTracking()
                .Where(up => sharePosts.Select(gsp => gsp.UserSharedId).Contains(up.UserId))
                .ToListAsync(cancellationToken);

            var avatarSharePhotos = await _context.AvataPhotos
                .AsNoTracking()
                .Where(ap => sharePosts.Select(gsp => gsp.UserSharedId).Contains(ap.UserId) && ap.IsUsed)
                .ToListAsync(cancellationToken);

            var groupPostShareIds = sharePosts.Select(gsp => gsp.GroupPostId).Distinct().ToList();
            var groupPostsForShare = await _context.GroupPosts
                .AsNoTracking()
                .Where(gp => groupPostShareIds.Contains(gp.GroupPostId))
                .ToListAsync(cancellationToken);

            var groupIds = groupPostsForShare.Select(gp => gp.GroupId).Distinct().ToList();
            var groups = await _context.GroupFpts
                .AsNoTracking()
                .Where(g => groupIds.Contains(g.GroupId))
                .ToListAsync(cancellationToken);

            foreach (var item in groupPosts)
            {
                var userProfile = userProfiles.FirstOrDefault(up => up.UserId == item.UserId);
                var userAvatar = avatarPhotos.FirstOrDefault(ap => ap.UserId == item.UserId);

                var reactNum = await _context.ReactGroupPosts.CountAsync(x => x.GroupPostId == item.GroupPostId);
                var commentNum = await _context.CommentGroupPosts.CountAsync(x => x.GroupPostId == item.GroupPostId && x.IsHide != true && x.IsBanned != true);
                var shareNum =  _context.SharePosts.Count(x => x.GroupPostId == item.GroupPostId) +
                    _context.GroupSharePosts.Count(x => x.GroupPostId == item.GroupPostId);

                var isReact = await _context.ReactGroupPosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.GroupPostId == item.GroupPostId && x.UserId == request.UserId);

                var topReact = await _context.ReactGroupPosts
                .AsNoTracking()
                .Include(x => x.ReactType)
                .Where(x => x.GroupPostId == item.GroupPostId)
                .GroupBy(x => x.ReactTypeId)
                .Select(g => new {
                    ReactTypeId = g.Key,
                    ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                    Count = g.Count()
                })
                .OrderByDescending(r => r.Count)
                .Take(2)
                .ToListAsync(cancellationToken);

                var groupPost = new GetGroupPostByGroupIdDTO
                {
                    PostId = item.GroupPostId,
                    UserId = item.UserId,
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    UpdateAt = item.UpdatedAt,
                    IsHide = item.IsHide,
                    IsShare = false,
                    IsBanned = item.IsBanned,
                    IsPending = item.IsPending,
                    GroupPostNumber = item.GroupPostNumber,
                    NumberGroupPost = item.NumberPost,
                    GroupPhoto = _mapper.Map<GroupPhotoDTO>(item.GroupPhoto),
                    GroupVideo = _mapper.Map<GroupVideoDTO>(item.GroupVideo),
                    GroupPostPhoto = item.GroupPostPhotos?.Select(upp => new GroupPostPhotoDTO
                    {
                        GroupPostPhotoId = upp.GroupPostPhotoId,
                        GroupPostId = upp.GroupPostId,
                        Content = upp.Content,
                        GroupPhotoId = upp.GroupPhotoId,
                        GroupStatusId = upp.GroupStatusId,
                        GroupPostPhotoNumber = upp.GroupPostPhotoNumber,
                        IsHide = upp.IsHide,
                        CreatedAt = upp.CreatedAt,
                        UpdatedAt = upp.UpdatedAt,
                        PostPosition = upp.PostPosition,
                        GroupPhoto = _mapper.Map<GroupPhotoDTO>(upp.GroupPhoto),
                    }).ToList(),
                    GroupPostVideo = item.GroupPostVideos?.Select(upp => new GroupPostVideoDTO
                    {
                        GroupPostVideoId = upp.GroupPostVideoId,
                        GroupPostId = upp.GroupPostId,
                        Content = upp.Content,
                        GroupVideoId = upp.GroupVideoId,
                        GroupStatusId = upp.GroupStatusId,
                        GroupPostVideoNumber = upp.GroupPostVideoNumber,
                        IsHide = upp.IsHide,
                        CreatedAt = upp.CreatedAt,
                        UpdatedAt = upp.UpdatedAt,
                        PostPosition = upp.PostPosition,
                        GroupVideo = _mapper.Map<GroupVideoDTO>(upp.GroupVideo),
                    }).ToList(),
                    UserName = $"{userProfile?.FirstName} {userProfile?.LastName}",
                    UserAvatar = _mapper.Map<GetUserAvatar>(userAvatar),
                    GroupStatus = new GetGroupStatusDTO
                    {
                        GroupStatusId = item.GroupStatusId,
                        GroupStatusName = item.GroupStatus.GroupStatusName,
                    },
                    GroupId = item.GroupId,
                    GroupName = item.Group.GroupName,
                    GroupCorverImage = item.Group.CoverImage,
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
                    },
                    EdgeRank = GetEdgeRankAlo.GetEdgeRank(reactNum, commentNum, shareNum, item.CreatedAt ?? DateTime.Now)
                };

                combine.Add(groupPost);
            }

            foreach (var item in sharePosts)
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == item.UserId);
                var userAvatar = await _context.AvataPhotos.FirstOrDefaultAsync(ap => ap.UserId == item.UserId && ap.IsUsed == true);

                var userShareProfile = userShareProfiles.FirstOrDefault(up => up.UserId == item.UserSharedId);
                var avatarShare = avatarSharePhotos.FirstOrDefault(ap => ap.UserId == item.UserSharedId);

                var reactNum = await _context.ReactGroupSharePosts.CountAsync(x => x.GroupSharePostId == item.GroupSharePostId);
                var commentNum = await _context.CommentGroupSharePosts.CountAsync(x => x.GroupSharePostId == item.GroupSharePostId && x.IsHide != true && x.IsBanned != true);

                var groupPost = groupPostsForShare.FirstOrDefault(gp => gp.GroupPostId == item.GroupPostId);
                var group = groups.FirstOrDefault(g => g.GroupId == groupPost?.GroupId);

                var isReact = await _context.ReactGroupSharePosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.GroupSharePostId == item.GroupSharePostId && x.UserId == request.UserId);

                var topReact = await _context.ReactGroupSharePosts
                .AsNoTracking()
                .Include(x => x.ReactType)
                .Where(x => x.GroupSharePostId == item.GroupSharePostId)
                .GroupBy(x => x.ReactTypeId)
                .Select(g => new {
                    ReactTypeId = g.Key,
                    ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                    Count = g.Count()
                })
                .OrderByDescending(r => r.Count)
                .Take(2)
                .ToListAsync(cancellationToken);

                var groupSharePost = new GetGroupPostByGroupIdDTO
                {
                    PostId = item.GroupSharePostId,
                    UserId = item.UserId,
                    Content = item.Content,
                    UserPostShareId = item.UserPostId,
                    UserPostPhotoShareId = item.UserPostPhotoId,
                    UserPostVideoShareId = item.UserPostVideoId,
                    GroupPostShareId = item.GroupPostId,
                    GroupPostPhotoShareId = item.GroupPostPhotoId,
                    GroupPostVideoShareId = item.GroupPostVideoId,
                    SharedToUserId = item.SharedToUserId,
                    CreatedAt = item.CreateDate,
                    UpdateAt = item.UpdateDate,
                    IsHide = item.IsHide,
                    IsBanned = item.IsBanned,
                    IsShare = true,
                    IsPending = item.IsPending,
                    GroupPostShare = _mapper.Map<GroupPostDTO>(item.GroupPost),
                    GroupPostPhotoShare = _mapper.Map<GroupPostPhotoDTO>(item.GroupPostPhoto),
                    GroupPostVideoShare = _mapper.Map<GroupPostVideoDTO>(item.GroupPostVideo),
                    UserPostShare = _mapper.Map<UserPostDTO>(item.UserPost),
                    UserPostPhotoShare = _mapper.Map<UserPostPhotoDTO>(item.UserPostPhoto),
                    UserPostVideoShare = _mapper.Map<UserPostVideoDTO>(item.UserPostVideo),
                    UserNameShare = $"{userShareProfile?.FirstName} {userShareProfile?.LastName}",
                    UserAvatarShare = _mapper.Map<GetUserAvatar>(avatarShare),
                    GroupShareId = group?.GroupId,
                    GroupShareName = group?.GroupName,
                    GroupShareCorverImage = group?.CoverImage,
                    UserName = $"{userProfile?.FirstName} {userProfile?.LastName}",
                    UserAvatar = _mapper.Map<GetUserAvatar>(userAvatar),
                    GroupStatus = new GetGroupStatusDTO
                    {
                        GroupStatusId = item.GroupStatusId ?? Guid.Empty,
                        GroupStatusName = item.GroupStatus.GroupStatusName
                    },
                    GroupId = item.GroupId,
                    GroupName = item.Group.GroupName,
                    GroupCorverImage = item.Group.CoverImage,
                    ReactCount = new DTO.ReactDTO.ReactCount
                    {
                        ReactNumber = reactNum,
                        CommentNumber = commentNum,
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
                    EdgeRank = GetEdgeRankAlo.GetEdgeRank(reactNum, commentNum, 0, item.CreateDate ?? DateTime.Now)
                };

                combine.Add(groupSharePost);
            }

            var getgrouppost = new GetGroupPostByGroupIdResult();

            getgrouppost.totalPage = (int)Math.Ceiling((double)combine.Count() / request.PageSize);

            combine = ApplySortingAndPaging(combine, request.Type, request.Page, request.PageSize);

            getgrouppost.result = combine;
            return Result<GetGroupPostByGroupIdResult>.Success(getgrouppost);
        }
    }
}
