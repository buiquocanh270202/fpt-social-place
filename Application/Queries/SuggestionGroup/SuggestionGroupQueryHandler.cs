using Application.Queries.SuggestionGroup;
using AutoMapper;
using Core.CQRS.Query;
using Core.CQRS;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Domain.Exceptions;
using Domain.Enums;
using Application.DTO.GroupFPTDTO;
using Application.Queries.SuggestFriend;

public class SuggestionGroupQueryHandler : IQueryHandler<SuggestionGroupQuery, SuggestionGroupQueryResult>
{
    private readonly fptforumQueryContext _queryContext;
    private readonly IMapper _mapper;

    public SuggestionGroupQueryHandler(fptforumQueryContext queryContext, IMapper mapper)
    {
        _queryContext = queryContext;
        _mapper = mapper;
    }

    public async Task<Result<SuggestionGroupQueryResult>> Handle(SuggestionGroupQuery request, CancellationToken cancellationToken)
    {
        if (_queryContext == null)
        {
            throw new ErrorException(StatusCodeEnum.Context_Not_Found);
        }
        // Lấy thông tin người dùng và các bạn bè
        var user = await _queryContext.UserProfiles
            .Include(u => u.FriendUsers)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new ErrorException(StatusCodeEnum.U01_Not_Found);
        }

        // Lấy ID của các bạn bè
        var friendIds = await _queryContext.Friends
        .Where(f => (f.UserId == request.UserId || f.FriendId == request.UserId) && f.Confirm)
        .Select(f => f.UserId == request.UserId ? f.FriendId : f.UserId)
        .ToListAsync(cancellationToken);

        // Lấy danh sách nhóm mà người dùng đã tham gia
        var userJoinedGroupIds = await _queryContext.GroupMembers
            .Where(gm => gm.UserId == request.UserId && gm.Group.IsDelete != true)
            .Select(gm => gm.GroupId)
            .ToListAsync(cancellationToken);

        // Đếm số lượng nhóm theo từng group type
        var groupTypeGroupCount = (from gm in _queryContext.GroupMembers
                                   join g in _queryContext.GroupFpts on gm.GroupId equals g.GroupId
                                   where gm.UserId == request.UserId && gm.IsJoined && gm.Group.IsDelete != true
                                   group g by g.GroupTypeId into grouped
                                   orderby grouped.Count() descending
                                   select new
                                   {
                                       GroupType = grouped.Key,
                                       Count = grouped.Count()
                                   }).ToList();

        // Đếm số lượng bạn bè tham gia trong mỗi nhóm
        var friendsInGroups = (from gm in _queryContext.GroupMembers
                               join g in _queryContext.GroupFpts on gm.GroupId equals g.GroupId
                               join gs in _queryContext.GroupStatuses on g.GroupStatusId equals gs.GroupStatusId
                               where gm.UserId == request.UserId && gm.IsJoined && g.IsDelete != true
                               select new
                               {
                                   Group = g,
                                   FriendCount = _queryContext.GroupMembers.Count(gm2 => friendIds.Contains(gm2.UserId) && gm2.GroupId == g.GroupId),
                                   GroupStatusName = gs.GroupStatusName
                               }).ToList();

        // Sắp xếp các nhóm theo thứ tự ưu tiên
        var prioritizedGroups = friendsInGroups
                                .OrderByDescending(g => groupTypeGroupCount.FindIndex(gt => gt.GroupType == g.Group.GroupTypeId))
                                .ThenByDescending(g => g.FriendCount)
                                .Where(g => !userJoinedGroupIds.Contains(g.Group.GroupId) || _queryContext.GroupMembers
                                       .Where(gm => gm.UserId == request.UserId && gm.GroupId == g.Group.GroupId)
                                       .All(gm => gm.IsJoined == false))
                                .Where(x => x.Group.IsDelete != true)
                                .Select(g => g.Group)
                                .ToList();

        // Nếu không có nhóm nào mà người dùng tham gia hoặc không có bạn bè nào tham gia trong các nhóm đó, tìm các nhóm có trạng thái công khai
        if (!prioritizedGroups.Any())
        {
            prioritizedGroups = _queryContext.GroupFpts
                .Include(g => g.GroupStatus)
                .Where(g => g.GroupStatus != null && g.GroupStatus.GroupStatusName.Equals("Public"))
                .Where(g => !userJoinedGroupIds.Contains(g.GroupId))
                .Where(g => g.IsDelete != true)
                .ToList();
        }

        // Tạo danh sách SuggestGroupDTO
        var groups = prioritizedGroups.Select(g => new SuggestGroupDTO
        {
            GroupId = g.GroupId,
            GroupName = g.GroupName,
            NumberOfMember = _queryContext.GroupMembers.Count(m => m.GroupId == g.GroupId),
            GroupStatus = g.GroupStatus?.GroupStatusName ?? "Unknown", // Kiểm tra null và cung cấp giá trị mặc định
            CoverImage = g.CoverImage,
        }).ToList();

        if (!request.ShowAll)
        {
            groups = groups.Take(5).ToList();
        }

        // Trả về kết quả
        var result = new SuggestionGroupQueryResult
        {
            suggestGroupDTOs = groups
        };

        return Result<SuggestionGroupQueryResult>.Success(result);
    }
}

