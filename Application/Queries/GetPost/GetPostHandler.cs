using AutoMapper;
using Core.CQRS.Query;
using Core.CQRS;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.DTO.UserPostDTO;
using Core.Helper;
using Application.DTO.GetUserProfileDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using System.Runtime.Intrinsics.X86;
using Domain.CommandModels;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.Queries.GetUserPost;
using Application.DTO.ReactDTO;

namespace Application.Queries.GetPost
{
    public class GetPostHandler : IQueryHandler<GetPostQuery, GetPostResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetPostHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetPostResult>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            if (request.Page <= 0)
            {
                return Result<GetPostResult>.Failure("Page number must be greater than 0");
            }

            // Lấy listuserBlock
            var blockUserList = await _context.BlockUsers
                .Where(x => (x.UserId == request.UserId || x.UserIsBlockedId == request.UserId) && x.IsBlock == true)
                .Select(x => x.UserId == request.UserId ? x.UserIsBlockedId : x.UserId)
                .ToListAsync(cancellationToken);

            // Retrieve the list of friend UserIds
            var friendUserIds = await _context.Friends
                                              .Where(f => (f.UserId == request.UserId || f.FriendId == request.UserId) && f.Confirm == true)
                                              .Select(f => f.UserId == request.UserId ? f.FriendId : f.UserId)
                                              .ToListAsync(cancellationToken);

            var userStatuses = await _context.UserStatuses
                                              .Where(x => x.StatusName == "Public" || x.StatusName == "Friend")
                                              .ToListAsync(cancellationToken);

            var statusPublic = userStatuses.FirstOrDefault(x => x.StatusName == "Public");
            var statusFriend = userStatuses.FirstOrDefault(x => x.StatusName == "Friend");

            // Lấy ra các bài viết từ bạn bè
            var posts = await _context.UserPosts
                .AsNoTracking()
                .Include(p => p.UserStatus)
                .Include(p => p.Photo)
                .Include(p => p.Video)
                .Include(p => p.UserPostPhotos.Where(x => x.IsHide != true && x.IsBanned != true))
                    .ThenInclude(upp => upp.Photo)
                .Include(p => p.UserPostVideos.Where(x => x.IsHide != true && x.IsBanned != true))
                    .ThenInclude(upv => upv.Video)
                .Where(p => !blockUserList.Contains(p.UserId) && friendUserIds.Contains(p.UserId) && (p.UserStatusId == statusPublic.UserStatusId || p.UserStatusId == statusFriend.UserStatusId) 
                            && p.IsHide != true && p.IsBanned != true)
                .ToListAsync(cancellationToken);

            var combinePost = new List<GetPostDTO>();

            foreach (var item in posts)
            {
                var user = _context.UserProfiles.Where(x => x.UserId == item.UserId)
                                                .Select(x => x.FirstName + " " + x.LastName)
                                                .FirstOrDefault();
                var avatar = _context.AvataPhotos.FirstOrDefault(x => x.UserId == item.UserId && x.IsUsed == true);

                var reactNum = await _context.ReactPosts.CountAsync(x => x.UserPostId == item.UserPostId);
                var commentNum = await _context.CommentPosts.CountAsync(x => x.UserPostId == item.UserPostId && x.IsHide != true && x.IsBanned != true);
                var shareNum = _context.SharePosts.Count(x => x.UserPostId == item.UserPostId) +
                    _context.GroupSharePosts.Count(x => x.UserPostId == item.UserPostId);

                var isReact = await _context.ReactPosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.UserPostId == item.UserPostId && x.UserId == request.UserId);

                var topReact = await _context.ReactPosts
                .Include(x => x.ReactType)
                .Where(x => x.UserPostId == item.UserPostId)
                .GroupBy(x => x.ReactTypeId)
                .Select(g => new {
                    ReactTypeId = g.Key,
                    ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                    Count = g.Count()
                })
                .OrderByDescending(r => r.Count)
                .Take(2)
                .ToListAsync(cancellationToken);

                var post = new GetPostDTO
                {
                    PostId = item.UserPostId,
                    UserId = item.UserId,
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    UpdateAt = item.UpdatedAt,
                    IsHide = item.IsHide,
                    IsBanned = item.IsBanned,
                    IsShare = false,
                    IsGroupPost = false,
                    UserPostNumber = item.UserPostNumber,
                    UserStatusId = item.UserStatusId,
                    IsAvataPost = item.IsAvataPost,
                    IsCoverPhotoPost = item.IsCoverPhotoPost,
                    PhotoId = item.PhotoId,
                    VideoId = item.VideoId,
                    NumberPost = item.NumberPost,
                    Photo = _mapper.Map<PhotoDTO>(item.Photo),
                    Video = _mapper.Map<VideoDTO>(item.Video),
                    UserPostPhoto = item.UserPostPhotos?.Select(upp => new UserPostPhotoDTO
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
                    UserPostVideo = item.UserPostVideos?.Select(upp => new UserPostVideoDTO
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
                    UserName = user,
                    UserAvatar = _mapper.Map<GetUserAvatar>(avatar),
                    UserStatus = new GetUserStatusDTO
                    {
                        UserStatusId = item.UserStatusId,
                        UserStatusName = item.UserStatus.StatusName,
                    },
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
                    EdgeRank = GetEdgeRankAlo.GetEdgeRank(reactNum, commentNum, shareNum, item?.CreatedAt ?? DateTime.Now)
                };

                combinePost.Add(post);

            }

            // Lấy ra các userpostId có lượng tương tác nổi bật (force : 10)
            var outStanding = await _context.PostReactCounts
                .AsNoTracking()
                .Where(x => (x.ReactCount + x.CommentCount + x.ShareCount) > 10)
                .Select(x => x.UserPostId)
                .ToListAsync(cancellationToken);

            // Lấy ra các UserPost có lượng tương tác nổi bật
            var outStandingPosts = await _context.UserPosts
                .AsNoTracking()
                .Include(p => p.UserStatus)
                .Include(p => p.Photo)
                .Include(p => p.Video)
                .Include(p => p.UserPostPhotos.Where(x => x.IsHide != true && x.IsBanned != true))
                    .ThenInclude(upp => upp.Photo)
                .Include(p => p.UserPostVideos.Where(x => x.IsHide != true && x.IsBanned != true))
                    .ThenInclude(upv => upv.Video)
                .Where(p => outStanding.Contains(p.UserPostId) && p.UserId != request.UserId && !blockUserList.Contains(p.UserId) && p.UserStatusId == statusPublic.UserStatusId && p.IsHide != true && p.IsBanned != true)
                .ToListAsync(cancellationToken);

            foreach (var item in outStandingPosts)
            {
                var user = _context.UserProfiles.Where(x => x.UserId == item.UserId)
                                                .Select(x => x.FirstName + " " + x.LastName)
                                                .FirstOrDefault();
                var avatar = _context.AvataPhotos.FirstOrDefault(x => x.UserId == item.UserId && x.IsUsed == true);

                var reactNum = await _context.ReactPosts.CountAsync(x => x.UserPostId == item.UserPostId);
                var commentNum = await _context.CommentPosts.CountAsync(x => x.UserPostId == item.UserPostId && x.IsHide != true && x.IsBanned != true);
                var shareNum = _context.SharePosts.Count(x => x.UserPostId == item.UserPostId) +
                    _context.GroupSharePosts.Count(x => x.UserPostId == item.UserPostId);

                var isReact = await _context.ReactPosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.UserPostId == item.UserPostId && x.UserId == request.UserId);

                var topReact = await _context.ReactPosts
                .Include(x => x.ReactType)
                .Where(x => x.UserPostId == item.UserPostId)
                .GroupBy(x => x.ReactTypeId)
                .Select(g => new {
                    ReactTypeId = g.Key,
                    ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                    Count = g.Count()
                })
                .OrderByDescending(r => r.Count)
                .Take(2)
                .ToListAsync(cancellationToken);

                var post = new GetPostDTO
                {
                    PostId = item.UserPostId,
                    UserId = item.UserId,
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    UpdateAt = item.UpdatedAt,
                    IsHide = item.IsHide,
                    IsBanned = item.IsBanned,
                    IsShare = false,
                    IsGroupPost = false,
                    UserPostNumber = item.UserPostNumber,
                    UserStatusId = item.UserStatusId,
                    IsAvataPost = item.IsAvataPost,
                    IsCoverPhotoPost = item.IsCoverPhotoPost,
                    PhotoId = item.PhotoId,
                    VideoId = item.VideoId,
                    NumberPost = item.NumberPost,
                    Photo = _mapper.Map<PhotoDTO>(item.Photo),
                    Video = _mapper.Map<VideoDTO>(item.Video),
                    UserPostPhoto = item.UserPostPhotos?.Select(upp => new UserPostPhotoDTO
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
                    UserPostVideo = item.UserPostVideos?.Select(upp => new UserPostVideoDTO
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
                    UserName = user,
                    UserAvatar = _mapper.Map<GetUserAvatar>(avatar),
                    UserStatus = new GetUserStatusDTO
                    {
                        UserStatusId = item.UserStatusId,
                        UserStatusName = item.UserStatus.StatusName,
                    },
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
                    EdgeRank = GetEdgeRankAlo.GetEdgeRank(reactNum, commentNum, shareNum, item?.CreatedAt ?? DateTime.Now)
                };

                combinePost.Add(post);

            }

            // Lấy ra id của setting Group Status
            var groupStatus = await _context.GroupSettings
                                    .Where(x => x.GroupSettingName.Contains("Group Status"))
                                    .Select(x => x.GroupSettingId)
                                    .FirstOrDefaultAsync(cancellationToken);

            // Lấy ra id của GroupStatuc ở Public
            var groupStatusPublicId = await _context.GroupStatuses
                                    .Where(x => x.GroupStatusName.Contains("Public"))
                                    .Select(x => x.GroupStatusId)
                                    .FirstOrDefaultAsync (cancellationToken);

            // Lấy ra những group mà friend join nhưng ở chế độ public
            var groupStatusPublic = await _context.GroupSettingUses
                                    .Where(x => x.GroupSettingId == groupStatus && x.GroupStatusId == groupStatusPublicId)
                                    .Select(x => x.GroupId)
                                    .ToListAsync(cancellationToken);

            // Lấy ra id của những group mà user đã join hoặc là của những friend đã join nhưng ở chế độ public
            var groupMemberIds = await _context.GroupMembers
                                    .Where(x => (x.UserId == request.UserId && x.IsJoined == true) || (/*friendUserIds.Contains(x.UserId) && */groupStatusPublic.Contains(x.GroupId)) )
                                    .Select(x => x.GroupId)
                                    .ToListAsync(cancellationToken);

            // Truy vấn bảng GroupPost theo những thông tin cần tìm kiếm
            var groupPost = await _context.GroupPosts
                                    .Include(x => x.GroupStatus)
                                    .Include(x => x.Group)
                                    .Include(x => x.GroupPhoto)
                                    .Include(x => x.GroupVideo)
                                    .Include(x => x.GroupPostPhotos.Where(x => x.IsHide != true && x.IsBanned != true))
                                        .ThenInclude(x => x.GroupPhoto)
                                    .Include(x => x.GroupPostVideos.Where(x => x.IsHide != true && x.IsBanned != true))
                                        .ThenInclude(x => x.GroupVideo)
                                    .Where(x => (groupMemberIds.Contains((Guid)x.GroupId) && !blockUserList.Contains(x.UserId) && x.Group.IsDelete != true && x.IsHide != true && x.IsBanned != true && x.IsPending != true))
                                    .ToListAsync();

            foreach(var item  in groupPost)
            {
                var user = _context.UserProfiles.Where(x => x.UserId == item.UserId)
                                                .Select(x => x.FirstName + " " + x.LastName)
                                                .FirstOrDefault();
                var avatar = _context.AvataPhotos.FirstOrDefault(x => x.UserId == item.UserId && x.IsUsed == true);

                var reactNum = await _context.ReactGroupPosts.CountAsync(x => x.GroupPostId == item.GroupPostId);
                var commentNum = await _context.CommentGroupPosts.CountAsync(x => x.GroupPostId == item.GroupPostId && x.IsHide != true && x.IsBanned != true);
                var shareNum = _context.SharePosts.Count(x => x.GroupPostId == item.GroupPostId) +
                    _context.GroupSharePosts.Count(x => x.GroupPostId == item.GroupPostId);

                var isReact = await _context.ReactGroupPosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.GroupPostId == item.GroupPostId && x.UserId == request.UserId);

                var topReact = await _context.ReactGroupPosts
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
                var post = new GetPostDTO
                { 
                    PostId = item.GroupPostId,
                    UserId = item.UserId,
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    UpdateAt = item.UpdatedAt,
                    IsHide = item.IsHide,
                    IsBanned = item.IsBanned,
                    IsShare = false,
                    IsGroupPost = true,
                    GroupPostNumber = item.GroupPostNumber,
                    GroupStatus = new GetGroupStatusDTO {
                        GroupStatusId = item.GroupStatusId,
                        GroupStatusName = item.GroupStatus.GroupStatusName,
                    },
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
                    GroupId = item.GroupId,
                    GroupName = item.Group.GroupName,
                    GroupCorverImage = item.Group.CoverImage,
                    UserName = user,
                    UserAvatar = _mapper.Map<GetUserAvatar>(avatar),
                    ReactCount = new ReactCount {
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

                combinePost.Add(post);
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
                .Where(p => friendUserIds.Contains(p.UserId) && !blockUserList.Contains(p.UserId) && 
                            (p.UserStatusId == statusPublic.UserStatusId || p.UserStatusId == statusFriend.UserStatusId) && p.IsHide != true && p.IsBanned != true)
                .ToListAsync(cancellationToken);

            foreach (var item in sharePosts)
            {
                var user = _context.UserProfiles.Where(x => x.UserId == item.UserId)
                                                .Select(x => x.FirstName + " " + x.LastName)
                                                .FirstOrDefault();

                var avatar = _context.AvataPhotos.FirstOrDefault(x => x.UserId == item.UserId && x.IsUsed == true);

                var userShare = _context.UserProfiles
                    .Where(x => x.UserId == item.UserSharedId)
                    .Select(x => x.FirstName + " " + x.LastName)
                    .FirstOrDefault();

                var avtShare = _context.AvataPhotos.FirstOrDefault(x => x.UserId == item.UserSharedId && x.IsUsed == true);

                var groupId = _context.GroupPosts.Where(x => x.GroupPostId == item.GroupPostId).Select(x => x.GroupId).FirstOrDefault();
                var group = _context.GroupFpts.FirstOrDefault(x => x.GroupId == groupId);

                var reactNum = _context.ReactSharePosts.Count(x => x.SharePostId == item.SharePostId);
                var commentNum = _context.CommentSharePosts
                    .Count(x => x.SharePostId == item.SharePostId && x.IsHide != true && x.IsBanned != true);

                var isReact = await _context.ReactSharePosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.SharePostId == item.SharePostId && x.UserId == request.UserId);

                var topReact = await _context.ReactSharePosts
                .Include(x => x.ReactType)
                .Where(x => x.SharePostId == item.SharePostId)
                .GroupBy(x => x.ReactTypeId)
                .Select(g => new {
                    ReactTypeId = g.Key,
                    ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                    Count = g.Count()
                })
                .OrderByDescending(r => r.Count)
                .Take(2)
                .ToListAsync(cancellationToken);

                var post = new GetPostDTO
                {
                    PostId = item.SharePostId,
                    UserId = item.UserId,
                    Content = item.Content,
                    UserPostShareId = item.UserPostId,
                    UserPostPhotoShareId = item.UserPostPhotoId,
                    UserPostVideoShareId = item.UserPostVideoId,
                    GroupPostShareId = item.GroupPostId,
                    GroupPostPhotoShareId = item.GroupPostPhotoId,
                    GroupPostVideoShareId = item.GroupPostVideoId,
                    SharedToUserId = item.SharedToUserId,
                    CreatedAt = item.CreatedDate,
                    UpdateAt = item.UpdateDate,
                    IsHide = item.IsHide,
                    IsBanned = item.IsBanned,
                    IsShare = true,
                    IsGroupPost = false,
                    GroupPostShare = _mapper.Map<GroupPostDTO>(item.GroupPost),
                    GroupPostPhotoShare = _mapper.Map<GroupPostPhotoDTO>(item.GroupPostPhoto),
                    GroupPostVideoShare = _mapper.Map<GroupPostVideoDTO>(item.GroupPostVideo),
                    UserPostShare = _mapper.Map<UserPostDTO>(item.UserPost),
                    UserPostPhotoShare = _mapper.Map<UserPostPhotoDTO>(item.UserPostPhoto),
                    UserPostVideoShare = _mapper.Map<UserPostVideoDTO>(item.UserPostVideo),
                    UserNameShare = userShare,
                    UserAvatarShare = _mapper.Map<GetUserAvatar>(avtShare),
                    GroupShareId = group?.GroupId ?? null,
                    GroupShareName = group?.GroupName ?? null,
                    GroupShareCorverImage = group?.CoverImage ?? null,
                    UserName = user,
                    UserAvatar = _mapper.Map<GetUserAvatar>(avatar),
                    UserStatus = new DTO.GetUserProfileDTO.GetUserStatusDTO
                    {
                        UserStatusId = item.UserStatusId,
                        UserStatusName = item.UserStatus.StatusName
                    },
                    ReactCount = new DTO.ReactDTO.ReactCount
                    {
                        ReactNumber =  reactNum,
                        CommentNumber =  commentNum,
                        ShareNumber =  0,
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
                    EdgeRank = GetEdgeRankAlo.GetEdgeRank(reactNum, commentNum, 0, item.CreatedDate ?? DateTime.Now)
                };

                combinePost.Add(post);
            }

            var getpost = new GetPostResult();

            getpost.totalPage = (int)Math.Ceiling((double)combinePost.Count() / request.PageSize);

            combinePost = combinePost
                            .OrderByDescending(x => x.EdgeRank)
                            .ThenByDescending(x => x.CreatedAt)
                            .Skip((request.Page - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ToList();

            getpost.result = combinePost;

            return Result<GetPostResult>.Success(getpost);
        }


    }

}
