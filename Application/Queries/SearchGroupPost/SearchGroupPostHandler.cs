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
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.SearchGroupPost
{
    public class SearchGroupPostHandler : IQueryHandler<SearchGroupPostQuery, SearchGroupPostResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;
        public SearchGroupPostHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private bool SubstringMatch(string? content, string[] searchWords)
        {
            if (content == null) {
                return false;
            }
            var normalizedContent = content.RemoveDiacritics().ToLower();
            return searchWords.Any(word => normalizedContent.Contains(word));
        }

        private bool ContainsMostWords(string? content, string[] searchWords)
        {
            if (content == null)
            {
                return false;
            }
            var postWords = content.RemoveDiacritics().ToLower().Split(' ');
            return searchWords.Count(word => postWords.Contains(word)) > searchWords.Length / 2;
        }

        public async Task<Result<SearchGroupPostResult>> Handle(SearchGroupPostQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var blockUserList = await _context.BlockUsers
                .Where(x => (x.UserId == request.UserId || x.UserIsBlockedId == request.UserId) && x.IsBlock == true)
                .Select(x => x.UserId == request.UserId ? x.UserIsBlockedId : x.UserId)
                .ToListAsync(cancellationToken);


            var combine = new List<GetGroupPostByGroupIdDTO>();

            var normalizedSearchString = request.SearchString.RemoveDiacritics().ToLower();
            var searchWords = normalizedSearchString.SplitIntoWords();

            var groupPosts = await _context.GroupPosts
                                    .AsNoTracking()
                                    .Include(x => x.GroupStatus)
                                    .Include(x => x.GroupPhoto)
                                    .Include(x => x.GroupVideo)
                                    .Include(x => x.GroupPostPhotos.Where(x => x.IsHide != true && x.IsBanned != true))
                                        .ThenInclude(x => x.GroupPhoto)
                                    .Include(x => x.GroupPostVideos.Where(x => x.IsHide != true && x.IsBanned != true))
                                        .ThenInclude(x => x.GroupVideo)
                                    .Include(x => x.Group)
                                    .Where(x => x.GroupId == request.GroupId && !blockUserList.Contains(x.UserId) && x.IsHide != true && x.IsBanned != true && x.IsPending == false)
                                    .ToListAsync(cancellationToken);

            var sharePosts = await _context.GroupSharePosts
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
                .Where(p => p.GroupId == request.GroupId && !blockUserList.Contains(p.UserId) && p.IsHide != true && p.IsBanned != true && p.IsPending == false)
                .ToListAsync(cancellationToken);

            foreach (var post in groupPosts.Cast<object>().Concat(sharePosts.Cast<object>()))
            {
                bool isMatch = false;
                var content = string.Empty;

                if (post is GroupPost groupPost)
                {
                    content = groupPost.Content;

                    // Kiểm tra tìm kiếm
                    isMatch = SubstringMatch(content, searchWords) || ContainsMostWords(content, searchWords);

                    if (isMatch)
                    {
                        var userName = _context.UserProfiles.Where(x => x.UserId == groupPost.UserId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                        var userAvatar = _context.AvataPhotos
                            .Where(x => x.UserId == groupPost.UserId && x.IsUsed == true)
                            .Select(x => new GetUserAvatar {
                                AvataPhotosId = x.AvataPhotosId,
                                AvataPhotosUrl = x.AvataPhotosUrl,
                                UserStatusId = x.UserStatusId,
                            })
                            .FirstOrDefault();

                        var reactNum = await _context.ReactGroupPosts.CountAsync(x => x.GroupPostId == groupPost.GroupPostId);
                        var commentNum = await _context.CommentGroupPosts.CountAsync(x => x.GroupPostId == groupPost.GroupPostId && x.IsHide != true && x.IsBanned != true);
                        var shareNum = _context.SharePosts.Count(x => x.GroupPostId == groupPost.GroupPostId) +
                            _context.GroupSharePosts.Count(x => x.GroupPostId == groupPost.GroupPostId);

                        var isReact = await _context.ReactGroupPosts
                            .Include(x => x.ReactType)
                            .FirstOrDefaultAsync(x => x.GroupPostId == groupPost.GroupPostId && x.UserId == request.UserId);

                        var topReact = await _context.ReactGroupPosts
                            .Include(x => x.ReactType)
                            .Where(x => x.GroupPostId == groupPost.GroupPostId)
                            .GroupBy(x => x.ReactTypeId)
                            .Select(g => new {
                                ReactTypeId = g.Key,
                                ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                                Count = g.Count()
                            })
                            .OrderByDescending(r => r.Count)
                            .Take(2)
                            .ToListAsync(cancellationToken);

                        // Thông tin bài đăng nhóm
                        var grouppost = new GetGroupPostByGroupIdDTO
                        {
                            PostId = groupPost.GroupPostId,
                            UserId = groupPost.UserId,
                            Content = groupPost.Content,
                            CreatedAt = groupPost.CreatedAt,
                            UpdateAt = groupPost.UpdatedAt,
                            IsHide = groupPost.IsHide,
                            IsBanned = groupPost.IsBanned,
                            IsShare = false,
                            IsPending = groupPost.IsPending,
                            GroupPostNumber = groupPost.GroupPostNumber,
                            NumberGroupPost = groupPost.NumberPost,
                            GroupPhoto = _mapper.Map<GroupPhotoDTO>(groupPost.GroupPhoto),
                            GroupVideo = _mapper.Map<GroupVideoDTO>(groupPost.GroupVideo),
                            GroupPostPhoto = groupPost.GroupPostPhotos?.Select(upp => new GroupPostPhotoDTO
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
                            GroupPostVideo = groupPost.GroupPostVideos?.Select(upp => new GroupPostVideoDTO
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
                                GroupStatusId = groupPost.GroupStatusId,
                                GroupStatusName = groupPost.GroupStatus.GroupStatusName,
                            },
                            GroupId = groupPost.GroupId,
                            GroupName = groupPost.Group.GroupName,
                            GroupCorverImage = groupPost.Group.CoverImage,
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
                            EdgeRank = GetEdgeRankAlo.GetEdgeRank(reactNum, commentNum, shareNum, groupPost.CreatedAt ?? DateTime.Now)
                        };

                        combine.Add(grouppost);
                    }
                }
                else if (post is GroupSharePost sharePost)
                {
                    content = sharePost.Content;

                    // Kiểm tra tìm kiếm
                    isMatch = SubstringMatch(content, searchWords) || ContainsMostWords(content, searchWords);

                    if (isMatch)
                    {
                        var user = _context.UserProfiles
                    .AsNoTracking()
                    .Where(x => x.UserId == sharePost.UserId)
                    .Select(x => x.FirstName + " " + x.LastName)
                    .FirstOrDefault();

                        var avatar = _context.AvataPhotos.FirstOrDefault(x => x.UserId == sharePost.UserId && x.IsUsed == true);

                        var userShare = _context.UserProfiles
                            .AsNoTracking()
                            .Where(x => x.UserId == sharePost.UserSharedId)
                            .Select(x => x.FirstName + " " + x.LastName)
                            .FirstOrDefault();

                        var avtShare = _context.AvataPhotos
                            .AsNoTracking()
                            .Where(x => x.UserId == sharePost.UserSharedId && x.IsUsed == true)
                            .Select(x => new GetUserAvatar
                            {
                                AvataPhotosId = x.AvataPhotosId,
                                AvataPhotosUrl = x.AvataPhotosUrl,
                                UserStatusId = x.UserStatusId,
                            })
                            .FirstOrDefault();

                        var groupId = _context.GroupPosts
                            .AsNoTracking()
                            .Where(x => x.GroupPostId == sharePost.GroupPostId).Select(x => x.GroupId).FirstOrDefault();
                        var group = _context.GroupFpts
                            .AsNoTracking()
                            .FirstOrDefault(x => x.GroupId == groupId);

                        var reactNum = await _context.ReactGroupSharePosts.CountAsync(x => x.GroupSharePostId == sharePost.GroupSharePostId);
                        var commentNum = await _context.CommentGroupSharePosts.CountAsync(x => x.GroupSharePostId == sharePost.GroupSharePostId && x.IsHide != true && x.IsBanned != true);

                        var isReact = await _context.ReactGroupSharePosts
                            .Include(x => x.ReactType)
                            .FirstOrDefaultAsync(x => x.GroupSharePostId == sharePost.GroupSharePostId && x.UserId == request.UserId);

                        var topReact = await _context.ReactGroupSharePosts
                            .Include(x => x.ReactType)
                            .Where(x => x.GroupSharePostId == sharePost.GroupSharePostId)
                            .GroupBy(x => x.ReactTypeId)
                            .Select(g => new {
                                ReactTypeId = g.Key,
                                ReactTypeName = g.First().ReactType.ReactTypeName, // Assuming ReactType has a Name property
                                Count = g.Count()
                            })
                            .OrderByDescending(r => r.Count)
                            .Take(2)
                            .ToListAsync(cancellationToken);

                        // Thông tin bài đăng chia sẻ
                        var sharepost = new GetGroupPostByGroupIdDTO
                        {
                            PostId = sharePost.GroupSharePostId,
                            UserId = sharePost.UserId,
                            Content = sharePost.Content,
                            CreatedAt = sharePost.CreateDate,
                            UpdateAt = sharePost.UpdateDate,
                            IsHide = sharePost.IsHide,
                            IsBanned = sharePost.IsBanned,
                            IsShare = true,
                            IsPending = sharePost.IsPending,
                            UserPostShareId = sharePost.UserPostId,
                            UserPostPhotoShareId = sharePost.UserPostPhotoId,
                            UserPostVideoShareId = sharePost.UserPostVideoId,
                            GroupPostShareId = sharePost.GroupPostId,
                            GroupPostPhotoShareId = sharePost.GroupPostPhotoId,
                            GroupPostVideoShareId = sharePost.GroupPostVideoId,
                            GroupPostShare = _mapper.Map<GroupPostDTO>(sharePost.GroupPost),
                            GroupPostPhotoShare = _mapper.Map<GroupPostPhotoDTO>(sharePost.GroupPostPhoto),
                            GroupPostVideoShare = _mapper.Map<GroupPostVideoDTO>(sharePost.GroupPostVideo),
                            UserPostShare = _mapper.Map<UserPostDTO>(sharePost.UserPost),
                            UserPostPhotoShare = _mapper.Map<UserPostPhotoDTO>(sharePost.UserPostPhoto),
                            UserPostVideoShare = _mapper.Map<UserPostVideoDTO>(sharePost.UserPostVideo),
                            UserNameShare = userShare,
                            UserAvatarShare = avtShare,
                            GroupShareId = group?.GroupId ?? null,
                            GroupShareName = group?.GroupName ?? null,
                            GroupShareCorverImage = group?.CoverImage ?? null,
                            UserName = user,
                            UserAvatar = _mapper.Map<GetUserAvatar>(avatar),
                            GroupStatus = new GetGroupStatusDTO
                            {
                                GroupStatusId = (Guid)sharePost.GroupStatusId,
                                GroupStatusName = sharePost.GroupStatus.GroupStatusName
                            },
                            ReactCount = new DTO.ReactDTO.ReactCount
                            {
                                ReactNumber = reactNum,
                                CommentNumber = commentNum,
                                ShareNumber = 0,
                            },
                            EdgeRank = GetEdgeRankAlo.GetEdgeRank(reactNum, commentNum, 0, sharePost.CreateDate ?? DateTime.Now)
                        };

                        combine.Add(sharepost);
                    }
                }
            }
            var searchgrouppost = new SearchGroupPostResult();

            searchgrouppost.totalPage = (int)Math.Ceiling((double)combine.Count() / request.PageSize);

            // Sắp xếp theo ngày đăng
            searchgrouppost.result = combine
                .OrderByDescending(p => p.EdgeRank)
                .ThenByDescending(p => p.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Trả về kết quả
            return Result<SearchGroupPostResult>.Success(searchgrouppost);
        }
    }
}
