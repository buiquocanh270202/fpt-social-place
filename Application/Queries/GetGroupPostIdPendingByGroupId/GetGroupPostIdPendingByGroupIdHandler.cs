using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Queries.GetGroupPostByGroupId;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Core.Helper;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupPostIdPendingByGroupId
{
    public class GetGroupPostIdPendingByGroupIdHandler :IQueryHandler<GetGroupPostIdPendingByGroupIdQuery, List<GetGroupPostIdPendingByGroupIdResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetGroupPostIdPendingByGroupIdHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<GetGroupPostIdPendingByGroupIdResult>>> Handle(GetGroupPostIdPendingByGroupIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            List<GetGroupPostIdPendingByGroupIdResult> combine = new List<GetGroupPostIdPendingByGroupIdResult>();

            var groupPost = await _context.GroupPosts
                                    .AsNoTracking()
                                    .Include(x => x.GroupStatus)
                                    .Include(x => x.GroupPhoto)
                                    .Include(x => x.GroupVideo)
                                    .Include(x => x.GroupPostPhotos.Where(x => x.IsHide != true && x.IsBanned != true))
                                        .ThenInclude(x => x.GroupPhoto)
                                    .Include(x => x.GroupPostVideos.Where(x => x.IsHide != true && x.IsBanned != true))
                                        .ThenInclude(x => x.GroupVideo)
                                    .Include(x => x.Group)
                                    .Where(x => x.GroupId == request.GroupId && x.IsHide != true && x.IsBanned != true && x.IsPending == true)
                                    .ToListAsync(cancellationToken);

            foreach (var item in groupPost)
            {
                var userName = _context.UserProfiles.Where(x => x.UserId == item.UserId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                var userAvatar = _context.AvataPhotos.Where(x => x.UserId == item.UserId && x.IsUsed == true).FirstOrDefault();
                var react = _context.GroupPostReactCounts.FirstOrDefault(x => x.GroupPostId == item.GroupPostId);

                combine.Add(new GetGroupPostIdPendingByGroupIdResult
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
                    UserName = userName,
                    UserAvatar = _mapper.Map<GetUserAvatar>(userAvatar),
                    GroupStatus = new GetGroupStatusDTO
                    {
                        GroupStatusId = item.GroupStatusId,
                        GroupStatusName = item.GroupStatus.GroupStatusName,
                    },
                    GroupId = item.GroupId,
                    GroupName = item.Group.GroupName,
                    GroupCorverImage = item.Group.CoverImage,
                });
            }

            var sharePost = await _context.GroupSharePosts
                .AsNoTracking()
                .Include(x => x.GroupStatus)
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
                    .ThenInclude(x => x.GroupPostPhotos)
                        .ThenInclude(x => x.GroupPhoto)
                .Include(x => x.GroupPost)
                    .ThenInclude(x => x.GroupPostVideos)
                        .ThenInclude(x => x.GroupVideo)
                .Include(x => x.GroupPostPhoto)
                    .ThenInclude(x => x.GroupPhoto)
                .Include(x => x.GroupPostVideo)
                    .ThenInclude(x => x.GroupVideo)
                .Where(p => p.GroupId == request.GroupId && p.IsHide != true && p.IsBanned != true && p.IsPending == true)
                .ToListAsync(cancellationToken);

            foreach (var item in sharePost)
            {
                var user = _context.UserProfiles
                    .AsNoTracking()
                    .Where(x => x.UserId == item.UserId)
                    .Select(x => x.FirstName + " " + x.LastName)
                    .FirstOrDefault();

                var avatar = _context.AvataPhotos.FirstOrDefault(x => x.UserId == item.UserId && x.IsUsed == true);

                var userShare = _context.UserProfiles
                    .AsNoTracking()
                    .Where(x => x.UserId == item.UserSharedId)
                    .Select(x => x.FirstName + " " + x.LastName)
                    .FirstOrDefault();

                var avtShare = _context.AvataPhotos
                    .AsNoTracking()
                    .FirstOrDefault(x => x.UserId == item.UserSharedId && x.IsUsed == true);

                var groupId = _context.GroupPosts
                    .AsNoTracking()
                    .Where(x => x.GroupPostId == item.GroupPostId).Select(x => x.GroupId).FirstOrDefault();
                var group = _context.GroupFpts
                    .AsNoTracking()
                    .FirstOrDefault(x => x.GroupId == groupId);

                var reactNumber = _context.ReactGroupSharePosts
                    .AsNoTracking()
                    .Count(x => x.GroupSharePostId == item.GroupSharePostId);
                var commentNumber = _context.CommentGroupSharePosts
                    .AsNoTracking()
                    .Count(x => x.GroupSharePostId == item.GroupSharePostId);

                combine.Add(new GetGroupPostIdPendingByGroupIdResult
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
                    GroupStatus = new GetGroupStatusDTO
                    {
                        GroupStatusId = (Guid)item.GroupStatusId,
                        GroupStatusName = item.GroupStatus.GroupStatusName
                    },
                });
            }

            combine = combine.OrderByDescending(x => x.CreatedAt)
                            .Skip((request.Page - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ToList();
            
            return Result<List<GetGroupPostIdPendingByGroupIdResult>>.Success(combine);
        }
    }
}
