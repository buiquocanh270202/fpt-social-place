using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetOtherUserPostByUserId
{
    public class GetOtherUserPostByUserIdHandler : IQueryHandler<GetOtherUserPostByUserIdQuery, GetOtherUserPostByUserIdResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetOtherUserPostByUserIdHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetOtherUserPostByUserIdResult>> Handle(GetOtherUserPostByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            // Lấy ra listuser mà user chặn hoặc bị chặn
            var blockUserList = await _context.BlockUsers
                .Where(x => (x.UserId == request.UserId || x.UserIsBlockedId == request.UserId) && x.IsBlock == true)
                .Select(x => x.UserId == request.UserId ? x.UserIsBlockedId : x.UserId)
                .ToListAsync(cancellationToken);

            if(blockUserList.Contains((Guid)request.UserId) || blockUserList.Contains(request.OtherUserId))
            {
                throw new ErrorException(StatusCodeEnum.UP05_Can_Not_See_Content);
            }

            var combine = new List<GetOtherUserPostByUserId>();

            var idprofilestatus = await _context.Settings
                .AsNoTracking()
                .Where(x => x.SettingName.Contains("Profile Status"))
                .Select(x => x.SettingId)
                .FirstOrDefaultAsync();
            var idpublic = await _context.UserStatuses
                .AsNoTracking()
                .Where(x => x.StatusName.Contains("Public"))
                .Select(x => x.UserStatusId)
                .FirstOrDefaultAsync();
            var setting = await _context.UserSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.OtherUserId && x.SettingId == idprofilestatus && x.UserStatusId == idpublic);

            var isAdmin = await _context.AdminProfiles
                .AnyAsync(x => x.AdminId == request.UserId && x.Role.NameRole == "Societe-admin");


            if (!isAdmin && setting == null)
            {
                throw new ErrorException(StatusCodeEnum.PS01_Profile_Status_Private);
            }

            var isFriend = await _context.Friends
                            .AsNoTracking()
                            .AnyAsync(x => (x.UserId == request.UserId && x.FriendId == request.OtherUserId && x.Confirm == true)
                            || (x.UserId == request.OtherUserId && x.FriendId == request.UserId && x.Confirm == true));

            var sttNames = new List<string> { "Public" };

            if (isFriend || isAdmin)
            {
                sttNames.Add("Friend");
            }

            if (isAdmin)
            {
                sttNames.Add("Private");
            }

            var sttStatuses = await _context.UserStatuses
                .AsNoTracking()
                .Where(x => sttNames.Contains(x.StatusName))
                .ToListAsync();

            var sttStatusIds = sttStatuses.Select(x => x.UserStatusId).ToList();

            var userPosts = await _context.UserPosts
                .AsNoTracking()
                .Include(x => x.UserStatus)
                .Include(x => x.Photo)
                .Include(x => x.Video)
                .Include(x => x.UserPostPhotos.Where(x => x.IsHide != true))
                    .ThenInclude(x => x.Photo)
                .Include(x => x.UserPostVideos.Where(x => x.IsHide != true))
                    .ThenInclude(x => x.Video)
                .Where(x => x.UserId == request.OtherUserId && sttStatusIds.Contains(x.UserStatusId) && x.IsHide != true && x.IsBanned != true)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

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
                .Where(x => x.UserId == request.OtherUserId && sttStatusIds.Contains((Guid)x.UserStatusId) && x.IsHide != true && x.IsBanned != true)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync(cancellationToken);

            var listUserId = sharePosts.Select(x => x.UserSharedId).ToList();
            var avt = await _context.AvataPhotos
                .AsNoTracking()
                .Where(x => x.UserId == request.OtherUserId || listUserId.Contains(x.UserId))
                .ToListAsync();

            var userProfile = await _context.UserProfiles
                .AsNoTracking()
                .Select(x => new
                {
                    x.UserId,
                    FullName = $"{x.FirstName} {x.LastName}"
                })
                .Where(x => x.UserId == request.OtherUserId || listUserId.Contains(x.UserId))
                .ToListAsync(cancellationToken);

            var listUserpostId = userPosts.Select(x => x.UserPostId).ToList();

            foreach (var item in userPosts)
            {
                var userAvt = avt.FirstOrDefault(x => x.UserId == item.UserId && x.IsUsed == true);
                var userName = userProfile.Where(x => x.UserId == item.UserId).Select(x => x.FullName).FirstOrDefault();

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

                var post = new GetOtherUserPostByUserId
                {
                    PostId = item.UserPostId,
                    UserId = item.UserId,
                    Content = item.Content,
                    UserPostNumber = item.UserPostNumber,
                    UserStatus = new DTO.GetUserProfileDTO.GetUserStatusDTO
                    {
                        UserStatusId = item.UserStatusId,
                        UserStatusName = item.UserStatus.StatusName
                    },
                    IsAvataPost = item.IsAvataPost,
                    IsHide = item.IsHide,
                    IsBanned = item.IsBanned,
                    IsShare = false,
                    CreatedAt = item.CreatedAt,
                    UpdateAt = item.UpdatedAt,
                    IsCoverPhotoPost = item.IsCoverPhotoPost,
                    PhotoId = item.PhotoId,
                    VideoId = item.VideoId,
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
                    UserName = userName,
                    UserAvatar = _mapper.Map<GetUserAvatar>(userAvt),
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

                combine.Add(post);
            }

            foreach (var item in sharePosts)
            {
                var userAvt = avt.FirstOrDefault(x => x.UserId == item.UserId && x.IsUsed == true);
                var userName = userProfile.Where(x => x.UserId == item.UserId).Select(x => x.FullName).FirstOrDefault();

                var userShare = userProfile.Where(x => x.UserId == item.UserSharedId).Select(x => x.FullName).FirstOrDefault();
                var avtShare = avt.FirstOrDefault(x => x.UserId == item.UserId && x.IsUsed == true);

                var groupId = _context.GroupPosts.Where(x => x.GroupPostId == item.GroupPostId).Select(x => x.GroupId).FirstOrDefault();
                var group = _context.GroupFpts.Select(x => new {
                    x.GroupId,
                    x.GroupName,
                    x.CoverImage,
                })
                .FirstOrDefault(x => x.GroupId == groupId);

                var reactNum = await _context.ReactSharePosts.CountAsync(x => x.SharePostId == item.SharePostId);
                var commentNum = await _context.CommentSharePosts.CountAsync(x => x.SharePostId == item.SharePostId && x.IsHide != true && x.IsBanned != true);

                var isReact = await _context.ReactSharePosts
                    .AsNoTracking()
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

                var post = new GetOtherUserPostByUserId
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
                    UserName = userName,
                    UserAvatar = _mapper.Map<GetUserAvatar>(userAvt),
                    UserStatus = new DTO.GetUserProfileDTO.GetUserStatusDTO
                    {
                        UserStatusId = item.UserStatusId,
                        UserStatusName = item.UserStatus.StatusName
                    },
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
                    }
                };

                combine.Add(post);
            }

            var getotheruserpost = new GetOtherUserPostByUserIdResult();

            getotheruserpost.totalPage = (int)Math.Ceiling((double)combine.Count() / request.PageSize);

            combine = combine.OrderByDescending(x => x.CreatedAt)
                            .Skip((request.Page - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ToList();


            getotheruserpost.result = combine;
            return Result<GetOtherUserPostByUserIdResult>.Success(getotheruserpost);
        }
    }
}
