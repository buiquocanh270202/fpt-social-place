
using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.Queries.GetChildPost;
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
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetChildGroupPost
{
    public class GetChildGroupPostHandler : IQueryHandler<GetChildGroupPostQuery, GetChildGroupPostResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetChildGroupPostHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetChildGroupPostResult>> Handle(GetChildGroupPostQuery request, CancellationToken cancellationToken)
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
                .Where(x => (x.UserId == request.UserId || x.UserIsBlockedId == request.UserId) && x.IsBlock == true)
                .Select(x => x.UserId == request.UserId ? x.UserIsBlockedId : x.UserId)
                .ToListAsync(cancellationToken);

            var groupPostId = await _context.GroupPostPhotos
                                            .Where(x => x.GroupPostPhotoId == request.GroupPostMediaId)
                                            .Select(x => x.GroupPostId)
                                            .FirstOrDefaultAsync();

            if (groupPostId == Guid.Empty)
            {
                groupPostId = await _context.GroupPostVideos
                                            .Where(x => x.GroupPostVideoId == request.GroupPostMediaId)
                                            .Select(x => x.GroupPostId)
                                            .FirstOrDefaultAsync();
            }

            //Lấy UserId của bài post cha
            var userId = await _context.GroupPosts
                .Where(x => x.GroupPostId == groupPostId)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            var groupId = await _context.GroupPosts
                .Where(x => x.GroupPostId == groupPostId)
                .Select(x => x.GroupId)
                .FirstOrDefaultAsync(cancellationToken);

            var isAdmin = await _context.AdminProfiles
                .AnyAsync(x => x.AdminId == request.UserId && x.Role.NameRole == "Societe-admin");

            if (blockUserList.Contains(userId) && !isAdmin)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }

            var isJoin = await _context.GroupMembers
                    .AsNoTracking()
                    .AnyAsync(x => x.GroupId == groupId && x.UserId == request.UserId);

            var isPrivate = await _context.GroupPosts
                    .AsNoTracking()
                    .Include(x => x.GroupStatus)
                    .AnyAsync(x => x.GroupStatus.GroupStatusName == "Private" && x.GroupPostId == groupPostId && x.UserId != request.UserId && !isJoin);

            if (!isAdmin && isPrivate)
            {
                throw new ErrorException(StatusCodeEnum.GR16_User_Not_Exist_In_Group);
            }

            var combinedResults = new List<GetChildGroupPostResult>();

            var groupPostPhotos = await _context.GroupPostPhotos
                .Include(x => x.GroupPost)
                .ThenInclude(x => x.Group)
                .Include(x => x.GroupStatus)
                .Include(x => x.GroupPhoto)
                .Where(x => x.GroupPostId == groupPostId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            foreach (var photo in groupPostPhotos)
            {
                var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == photo.GroupPost.UserId);
                var avt = await _context.AvataPhotos.FirstOrDefaultAsync(x => x.UserId == photo.GroupPost.UserId && x.IsUsed == true);

                var reactNum = await _context.ReactGroupPhotoPosts.CountAsync(x => x.GroupPostPhotoId == photo.GroupPostPhotoId);
                var commentNum = await _context.CommentPhotoGroupPosts
                        .CountAsync(x => x.GroupPostPhotoId == photo.GroupPostPhotoId && x.IsHide != true && x.IsBanned != true);
                var shareNum = _context.GroupSharePosts.Count(x => x.GroupPostPhotoId == photo.GroupPostPhotoId && x.IsHide != true && x.IsBanned != true) +
                                        _context.SharePosts.Count(x => x.GroupPostPhotoId == photo.GroupPostPhotoId && x.IsHide != true && x.IsBanned != true);

                var isReact = await _context.ReactGroupPhotoPosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.GroupPostPhotoId == photo.GroupPostPhotoId && x.UserId == request.UserId);

                var topReact = await _context.ReactGroupPhotoPosts
                .AsNoTracking()
                .Include(x => x.ReactType)
                .Where(x => x.GroupPostPhotoId == photo.GroupPostPhotoId)
                .GroupBy(x => x.ReactTypeId)
                .Select(g => new {
                    ReactTypeId = g.Key,
                    ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                    Count = g.Count()
                })
                .OrderByDescending(r => r.Count)
                .Take(2)
                .ToListAsync(cancellationToken);

                var post = new GetChildGroupPostResult
                {
                    GroupPostMediaId = photo.GroupPostPhotoId,
                    GroupPostId = photo.GroupPostId,
                    GroupMediaId = photo.GroupPhotoId,
                    Content = photo.Content,
                    GroupPostMediaNumber = photo.GroupPostPhotoNumber,
                    Status = new GetGroupStatusDTO
                    {
                        GroupStatusId = photo.GroupStatusId,
                        GroupStatusName = photo.GroupStatus.GroupStatusName
                    },
                    IsHide = photo.IsHide,
                    CreatedAt = photo.CreatedAt,
                    UpdatedAt = photo.UpdatedAt,
                    PostPosition = photo.PostPosition,
                    GroupMediaType = "Photo",
                    GroupPhoto = _mapper.Map<GroupPhotoDTO>(photo.GroupPhoto),
                    UserId = photo.GroupPost.UserId,
                    FullName = user != null ? user.FirstName + " " + user.LastName : string.Empty,
                    Avatar = avt != null ? _mapper.Map<GetUserAvatar>(avt) : null,
                    GroupId = photo.GroupId,
                    GroupName = photo.Group?.GroupName,
                    GroupCoverImge = photo.Group?.CoverImage,
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

                combinedResults.Add(post);
            }

            var groupPostVideos = await _context.GroupPostVideos
                .Include(x => x.GroupPost)
                .ThenInclude(x => x.Group)
                .Include(x => x.GroupStatus)
                .Include(x => x.GroupVideo)
                .Where(x => x.GroupPostId == groupPostId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            foreach (var video in groupPostVideos)
            {
                var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == video.GroupPost.UserId);
                var avt = await _context.AvataPhotos.FirstOrDefaultAsync(x => x.UserId == video.GroupPost.UserId && x.IsUsed == true);

                var reactNum = await _context.ReactGroupVideoPosts.CountAsync(x => x.GroupPostVideoId == video.GroupPostVideoId);
                var commentNum = await _context.CommentGroupVideoPosts
                        .CountAsync(x => x.GroupPostVideoId == video.GroupPostVideoId && x.IsHide != true && x.IsBanned != true);
                var shareNum = _context.GroupSharePosts.Count(x => x.GroupPostVideoId == video.GroupPostVideoId && x.IsHide != true && x.IsBanned != true) +
                                        _context.SharePosts.Count(x => x.GroupPostVideoId == video.GroupPostVideoId && x.IsHide != true && x.IsBanned != true);

                var isReact = await _context.ReactGroupVideoPosts
                    .Include(x => x.ReactType)
                    .FirstOrDefaultAsync(x => x.GroupPostVideoId == video.GroupPostVideoId && x.UserId == request.UserId);

                var topReact = await _context.ReactGroupVideoPosts
                .AsNoTracking()
                .Include(x => x.ReactType)
                .Where(x => x.GroupPostVideoId == video.GroupPostVideoId)
                .GroupBy(x => x.ReactTypeId)
                .Select(g => new {
                    ReactTypeId = g.Key,
                    ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                    Count = g.Count()
                })
                .OrderByDescending(r => r.Count)
                .Take(2)
                .ToListAsync(cancellationToken);

                var post = new GetChildGroupPostResult
                {
                    GroupPostMediaId = video.GroupPostVideoId,
                    GroupPostId = video.GroupPostId,
                    GroupMediaId = video.GroupVideoId,
                    Content = video.Content,
                    GroupPostMediaNumber = video.GroupPostVideoNumber,
                    Status = new GetGroupStatusDTO
                    {
                        GroupStatusId = video.GroupStatusId,
                        GroupStatusName = video.GroupStatus.GroupStatusName
                    },
                    IsHide = video.IsHide,
                    CreatedAt = video.CreatedAt,
                    UpdatedAt = video.UpdatedAt,
                    PostPosition = video.PostPosition,
                    GroupMediaType = "Video",
                    GroupVideo = _mapper.Map<GroupVideoDTO>(video.GroupVideo),
                    UserId = video.GroupPost.UserId,
                    FullName = user != null ? user.FirstName + " " + user.LastName : string.Empty,
                    Avatar = avt != null ? _mapper.Map<GetUserAvatar>(avt) : null,
                    GroupId = video.GroupId,
                    GroupName = video.Group?.GroupName,
                    GroupCoverImge = video.Group?.CoverImage,
                    ReactCount = new DTO.ReactDTO.ReactCount
                    {
                        ReactNumber = reactNum,
                        CommentNumber = commentNum,
                        ShareNumber = shareNum,
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

                combinedResults.Add(post);
            }

            combinedResults = combinedResults.OrderBy(x => x.PostPosition).ToList();

            for (int i = 0; i < combinedResults.Count; i++)
            {
                if (i > 0)
                {
                    combinedResults[i].PreviousId = combinedResults[i - 1].GroupPostMediaId;
                    combinedResults[i].PreviousType = combinedResults[i - 1].GroupMediaType;
                }
                else
                {
                    combinedResults[i].PreviousId = combinedResults[combinedResults.Count - 1].GroupPostMediaId;
                    combinedResults[i].PreviousType = combinedResults[combinedResults.Count - 1].GroupMediaType;
                }

                if (i < combinedResults.Count - 1)
                {
                    combinedResults[i].NextId = combinedResults[i + 1].GroupPostMediaId;
                    combinedResults[i].NextType = combinedResults[i + 1].GroupMediaType;
                }
                else
                {
                    combinedResults[i].NextId = combinedResults[0].GroupPostMediaId;
                    combinedResults[i].NextType = combinedResults[0].GroupMediaType;
                }
            }

            var result = combinedResults.FirstOrDefault(x => x.GroupPostMediaId == request.GroupPostMediaId);
            if(result == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            return Result<GetChildGroupPostResult>.Success(result);
        }


    }
}
