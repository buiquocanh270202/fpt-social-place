using Application.DTO.FriendDTO;
using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupFPTDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Queries.FindUserByName;
using Application.Queries.GetGroupPostByGroupId;
using Application.Queries.GetPost;
using Application.Queries.GetUserPost;
using Application.Services;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.SearchFunction
{
    public class SearchFunctionQueryHandler : IQueryHandler<SearchFunctionQuery, SearchFunctionQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public SearchFunctionQueryHandler(fptforumQueryContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
        }
        private bool SubstringMatch(string? content, string[] searchWords)
        {
            if (content == null)
            {
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

        public async Task<Result<SearchFunctionQueryResult>> Handle(SearchFunctionQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (string.IsNullOrEmpty(request.SearchContent))
            {
                throw new ErrorException("Search content is required");
            }

            var queryResult = new SearchFunctionQueryResult();
            string searchContent = NormalizeString(request.SearchContent);

            switch (request.Type)
            {
                case "All": // Search across all entities
                    var groups = await searchGroup(request);
                    var users = await searchUsers(request);

                    queryResult.groups = groups.Take(3).ToList();
                    queryResult.userProfiles = users.Take(3).ToList();

                    var posts = await searchUserPosts(request);
                    var total = (int)Math.Ceiling((double)posts.Count() / request.PageSize);
                    queryResult.totalPage = total == 0 ? 1 : total;

                    queryResult.userPosts = posts
                                            .Skip((request.Page - 1) * request.PageSize)
                                            .Take(request.PageSize)
                                            .ToList();

                    break;

                case "Group": // Search groups only
                    var groupList = await searchGroup(request);

                    queryResult.groups = groupList
                                        .Skip((request.Page - 1) * request.PageSize)
                                        .Take(request.PageSize)
                                        .ToList();
                    queryResult.totalPage = (int)Math.Ceiling((double)groupList.Count() / request.PageSize);

                    break;

                case "User": // Search friends (UserProfiles) only
                    var userList = await searchUsers(request);

                    queryResult.totalPage = (int)Math.Ceiling((double)userList.Count() / request.PageSize);
                    queryResult.userProfiles = userList
                                .Skip((request.Page - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .ToList();

                    break;

                case "Post": // Search posts only
                    var postList = await searchUserPosts(request);

                    queryResult.totalPage = (int)Math.Ceiling((double)postList.Count() / request.PageSize);
                    queryResult.userPosts = postList
                                .Skip((request.Page - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .ToList();
                    break;
                // Consider adding a default case to handle invalid request types
                default:
                    throw new ErrorException("Type search is not vaid!");
            }

            return Result<SearchFunctionQueryResult>.Success(queryResult);
        }

        public async Task<List<GroupFPTDTO>> searchGroup (SearchFunctionQuery request)
        {
            if (string.IsNullOrEmpty(request.SearchContent))
            {
                throw new ErrorException("Search content is required");
            }

            var groupList = await _context.GroupFpts
                                                     .Where(x => x.IsDelete != true)
                                                     .ToListAsync();

            // Chuẩn hóa nội dung tìm kiếm
            var normalizedSearchString = request.SearchContent.RemoveDiacritics().ToLower();
            var searchWords = normalizedSearchString.SplitIntoWords();

            // Lọc danh sách các nhóm theo nội dung tìm kiếm
            var listFind = groupList
                            .Select(g => new
                            {
                                User = g,
                                NormalizedName = g.GroupName.RemoveDiacritics().ToLower(),
                                NameWords = g.GroupName.RemoveDiacritics().ToLower().SplitIntoWords(),
                                Permutations = g.GroupName.RemoveDiacritics().ToLower().SplitIntoWords().GetAllPermutations()
                            })
                            .Select(g => new
                            {
                                g.User,
                                g.NormalizedName,
                                g.NameWords,
                                g.Permutations,
                                ExactMatch = g.NormalizedName.Equals(normalizedSearchString),
                                NoDiacriticsMatch = g.NormalizedName == normalizedSearchString,
                                ReverseNameMatch = string.Join(" ", g.NameWords.Reverse()) == normalizedSearchString,
                                ReverseNoDiacriticsMatch = string.Join(" ", g.NameWords.Reverse()) == normalizedSearchString,
                                PermutationMatch = g.Permutations.Contains(normalizedSearchString),
                                ContainsMostWords = searchWords.Count(word => g.NameWords.Contains(word)),
                                ContainsAnyWords = searchWords.All(word => g.NameWords.Contains(word)),
                                SubstringMatch = searchWords.Any(word => g.NormalizedName.Contains(word))
                            })
                            .Where(u => u.ExactMatch || u.NoDiacriticsMatch || u.ReverseNameMatch ||
                                        u.ReverseNoDiacriticsMatch || u.PermutationMatch ||
                                        u.ContainsMostWords > 0 || u.ContainsAnyWords || u.SubstringMatch)
                            .OrderByDescending(u => u.ExactMatch)
                            .ThenByDescending(u => u.NoDiacriticsMatch)
                            .ThenByDescending(u => u.ReverseNameMatch)
                            .ThenByDescending(u => u.ReverseNoDiacriticsMatch)
                            .ThenByDescending(u => u.PermutationMatch)
                            .ThenByDescending(u => u.ContainsMostWords)
                            .ThenByDescending(u => u.ContainsAnyWords)
                            .ThenByDescending(u => u.SubstringMatch)
                            .ThenBy(u => u.NormalizedName)
                            .Select(u => u.User)
                            .ToList();

            var result = new List<GroupFPTDTO>();

            foreach (var group in listFind)
            {
                var newFind = new GroupFPTDTO
                {
                    GroupId = group.GroupId,
                    GroupNumber = group.GroupNumber,
                    GroupName = group.GroupName,
                    GroupDescription  = group.GroupDescription,
                    CreatedById = group.CreatedById,
                    CreatedDate = group.CreatedDate,
                    UpdateAt = group.UpdateAt,
                    GroupTypeId = group.GroupTypeId,
                    CoverImage = group.CoverImage,
                    GroupStatusId = group.GroupStatusId,
                    isJoined = false
                };

                var isJoin = await _context.GroupMembers.AnyAsync(x => x.GroupId == group.GroupId && x.UserId == request.UserId);
                if (isJoin)
                {
                    newFind.isJoined = true;
                }
                result.Add(newFind);
            }

            return result;
        }

        public async Task<List<UserDTO>> searchUsers(SearchFunctionQuery request)
        {
            if (string.IsNullOrEmpty(request.SearchContent))
            {
                throw new ErrorException("Search content is required");
            }

            var normalizedSearchString = request.SearchContent.RemoveDiacritics().ToLower();
            var searchWords = normalizedSearchString.SplitIntoWords();

            var users = await _context.UserProfiles.Include(x => x.AvataPhotos).Where(x => x.IsActive == true).ToListAsync();

            var listFind = users
                            .Select(user => new
                            {
                                User = user,
                                NormalizedName = user.FullName.RemoveDiacritics().ToLower(),
                                NameWords = user.FullName.RemoveDiacritics().ToLower().SplitIntoWords(),
                                Permutations = user.FullName.RemoveDiacritics().ToLower().SplitIntoWords().GetAllPermutations()
                            })
                            .Select(user => new
                            {
                                user.User,
                                user.NormalizedName,
                                user.NameWords,
                                user.Permutations,
                                ExactMatch = user.NormalizedName.Equals(normalizedSearchString),
                                NoDiacriticsMatch = user.NormalizedName == normalizedSearchString,
                                ReverseNameMatch = string.Join(" ", user.NameWords.Reverse()) == normalizedSearchString,
                                ReverseNoDiacriticsMatch = string.Join(" ", user.NameWords.Reverse()) == normalizedSearchString,
                                PermutationMatch = user.Permutations.Contains(normalizedSearchString),
                                ContainsMostWords = searchWords.Count(word => user.NameWords.Contains(word)),
                                ContainsAnyWords = searchWords.All(word => user.NameWords.Contains(word)),
                                SubstringMatch = searchWords.Any(word => user.NormalizedName.Contains(word))
                            })
                            .Where(u => u.ExactMatch || u.NoDiacriticsMatch || u.ReverseNameMatch ||
                                        u.ReverseNoDiacriticsMatch || u.PermutationMatch ||
                                        u.ContainsMostWords > 0 || u.ContainsAnyWords || u.SubstringMatch)
                            .OrderByDescending(u => u.ExactMatch)
                            .ThenByDescending(u => u.NoDiacriticsMatch)
                            .ThenByDescending(u => u.ReverseNameMatch)
                            .ThenByDescending(u => u.ReverseNoDiacriticsMatch)
                            .ThenByDescending(u => u.PermutationMatch)
                            .ThenByDescending(u => u.ContainsMostWords)
                            .ThenByDescending(u => u.ContainsAnyWords)
                            .ThenByDescending(u => u.SubstringMatch)
                            .ThenBy(u => u.NormalizedName)
                            .Select(u => u.User)
                            .ToList();

            var result = new List<UserDTO>();

            foreach (var user in listFind)
            {
                var newFind = new UserDTO
                {
                    UserName = user.FullName,
                    UserId = user.UserId,
                    AvataUrl = user.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl,
                    isFriended =  false
                };

                var blockUserList = await _context.BlockUsers
                .Where(x => (x.UserId == request.UserId || x.UserIsBlockedId == request.UserId) && x.IsBlock == true)
                .Select(x => x.UserId == request.UserId ? x.UserIsBlockedId : x.UserId)
                .ToListAsync();

                var isFriend = await _context.Friends.AnyAsync(x => (x.UserId == request.UserId && x.FriendId == user.UserId)
                                                                        || (x.UserId == user.UserId && x.FriendId == request.UserId)
                                                                        && x.Confirm == true);
                if (isFriend)
                {
                    newFind.isFriended = true;
                }
                if (!blockUserList.Contains(user.UserId))
                {
                    result.Add(newFind);
                }
            }
                return result;
        }

        public async Task<List<GetPostDTO>> searchUserPosts(SearchFunctionQuery request)
        {
            if (string.IsNullOrEmpty(request.SearchContent))
            {
                throw new ErrorException("Search content is required");
            }

            var blockUserList = await _context.BlockUsers
                .Where(x => (x.UserId == request.UserId || x.UserIsBlockedId == request.UserId) && x.IsBlock == true)
                .Select(x => x.UserId == request.UserId ? x.UserIsBlockedId : x.UserId)
                .ToListAsync();


            //var combine = new List<GetGroupPostByGroupIdDTO>();

            var normalizedSearchString = request.SearchContent.RemoveDiacritics().ToLower();
            var searchWords = normalizedSearchString.SplitIntoWords();

            // Retrieve the list of friend UserIds
            var friendUserIds = await _context.Friends
                                              .Where(f => (f.UserId == request.UserId || f.FriendId == request.UserId) && f.Confirm == true)
                                              .Select(f => f.UserId == request.UserId ? f.FriendId : f.UserId)
                                              .ToListAsync();

            var userStatuses = await _context.UserStatuses
                                              .Where(x => x.StatusName == "Public" || x.StatusName == "Friend")
                                              .ToListAsync();

            var statusPublic = userStatuses.FirstOrDefault(x => x.StatusName == "Public");
            var statusFriend = userStatuses.FirstOrDefault(x => x.StatusName == "Friend");

            var posts = await _context.UserPosts
                .AsNoTracking()
                .Include(p => p.UserStatus)
                .Include(p => p.Photo)
                .Include(p => p.Video)
                .Include(p => p.UserPostPhotos.Where(x => x.IsHide != true && x.IsBanned != true))
                    .ThenInclude(upp => upp.Photo)
                .Include(p => p.UserPostVideos.Where(x => x.IsHide != true && x.IsBanned != true))
                    .ThenInclude(upv => upv.Video)
                .Where(p => friendUserIds.Contains(p.UserId) && !blockUserList.Contains(p.UserId) &&
                            (p.UserStatusId == statusPublic.UserStatusId || p.UserStatusId == statusFriend.UserStatusId) && p.IsHide != true && p.IsBanned != true)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            // Lấy ra id của setting Group Status
            var groupStatus = await _context.GroupSettings
                                    .AsNoTracking()
                                    .Where(x => x.GroupSettingName.Contains("Group Status"))
                                    .Select(x => x.GroupSettingId)
                                    .FirstOrDefaultAsync();

            // Lấy ra id của GroupStatuc ở Public
            var groupStatusPublicId = await _context.GroupStatuses
                                    .AsNoTracking()
                                    .Where(x => x.GroupStatusName.Contains("Public"))
                                    .Select(x => x.GroupStatusId)
                                    .FirstOrDefaultAsync();

            // Lấy ra những group mà friend join nhưng ở chế độ public
            var groupStatusPublic = await _context.GroupSettingUses
                                    .AsNoTracking()
                                    .Where(x => x.GroupSettingId == groupStatus && x.GroupStatusId == groupStatusPublicId)
                                    .Select(x => x.GroupId)
                                    .ToListAsync();

            // Lấy ra id của những group mà user đã join hoặc là của những friend đã join nhưng ở chế độ public
            var groupMemberIds = await _context.GroupMembers
                                    .Where(x => (x.UserId == request.UserId && x.IsJoined == true) || (/*friendUserIds.Contains(x.UserId) && */groupStatusPublic.Contains(x.GroupId)))
                                    .Select(x => x.GroupId)
                                    .ToListAsync();

            // Truy vấn bảng GroupPost theo những thông tin cần tìm kiếm
            var groupPost = await _context.GroupPosts
                                    .AsNoTracking()
                                    .Include(x => x.GroupStatus)
                                    .Include(x => x.Group)
                                    .Include(x => x.GroupPhoto)
                                    .Include(x => x.GroupVideo)
                                    .Include(x => x.GroupPostPhotos.Where(x => x.IsHide != true && x.IsBanned != true))
                                        .ThenInclude(x => x.GroupPhoto)
                                    .Include(x => x.GroupPostVideos.Where(x => x.IsHide != true && x.IsBanned != true))
                                        .ThenInclude(x => x.GroupVideo)
                                    .Where(x => (groupMemberIds.Contains((Guid)x.GroupId) && !blockUserList.Contains(x.UserId) && x.Group.IsDelete != true && x.IsHide != true && x.IsBanned != true))
                                    .ToListAsync();

            var combinePost = new List<GetPostDTO>();

            foreach (var item in posts)
            {
                bool isMatch = false;
                var content = string.Empty;

                content = item.Content;
                isMatch = SubstringMatch(content, searchWords) || ContainsMostWords(content, searchWords);

                if (isMatch)
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
                    .AsNoTracking()
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
                    .ToListAsync();

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
            }

            foreach (var item in groupPost)
            {
                bool isMatch = false;
                var content = string.Empty;

                content = item.Content;
                isMatch = SubstringMatch(content, searchWords) || ContainsMostWords(content, searchWords);

                if (isMatch)
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
                    .ToListAsync();
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
                        GroupStatus = new GetGroupStatusDTO
                        {
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

                    combinePost.Add(post);
                }
            }

            // Lấy ra các userpostId có lượng tương tác nổi bật (force : 10)
            var outStanding = await _context.PostReactCounts
                .AsNoTracking()
                .Where(x => (x.ReactCount + x.CommentCount + x.ShareCount) > 10)
                .Select(x => x.UserPostId)
                .ToListAsync();

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
                .ToListAsync();

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
                .ToListAsync();

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
            return combinePost;
        }

        static string NormalizeString(string input)
        {
            // Chuyển đổi thành chữ thường
            input = input.ToLower();

            // Loại bỏ dấu tiếng Việt
            input = RemoveDiacritics(input);

            return input;
        }

        static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
