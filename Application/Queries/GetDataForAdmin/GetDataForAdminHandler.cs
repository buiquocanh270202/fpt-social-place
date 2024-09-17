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

namespace Application.Queries.GetDataForAdmin
{
    public class GetDataForAdminHandler : IQueryHandler<GetDataForAdmin, GetDataForAdminResult>
    {
        private readonly fptforumQueryContext _context;

        public GetDataForAdminHandler(fptforumQueryContext context) {
            _context = context;
        }
        public async Task<Result<GetDataForAdminResult>> Handle(GetDataForAdmin request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userCount = await _context.UserProfiles
                .AsNoTracking()
                .CountAsync();

            var activeUserCount = await _context.UserProfiles
                .AsNoTracking()
                .CountAsync(x => x.IsActive == true);

            var inactiveUserCount = await _context.UserProfiles
                .AsNoTracking()
                .CountAsync(x => x.IsActive == false);

            var userPosrCount = await _context.UserPosts
                .AsNoTracking()
                .CountAsync();

            var groupPostCount = await _context.GroupPosts
                .AsNoTracking()
                .CountAsync();

            var sharePostCount = await _context.SharePosts
                .AsNoTracking()
                .CountAsync();

            var groupSharePostCount = await _context.GroupSharePosts
                .AsNoTracking()
                .CountAsync();

            var groupCount = await _context.GroupFpts
                .AsNoTracking()
                .CountAsync();

            var result = new GetDataForAdminResult {
                NumberOfUser = userCount,
                NumberOfActiveUser = activeUserCount,
                NumberOfInactiveUser = inactiveUserCount,
                NumberOfPost = userPosrCount + groupPostCount + sharePostCount + groupSharePostCount,
                NumberOfGroup = groupCount,
            };

            return Result<GetDataForAdminResult>.Success(result);
        }
    }
}
