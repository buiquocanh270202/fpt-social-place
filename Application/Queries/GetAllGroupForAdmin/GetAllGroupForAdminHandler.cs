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

namespace Application.Queries.GetAllGroupForAdmin
{
    public class GetAllGroupForAdminHandler : IQueryHandler<GetAllGroupForAdmin, GetAllGroupForAdminResult>
    {
        private readonly fptforumQueryContext _context;

        public GetAllGroupForAdminHandler(fptforumQueryContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllGroupForAdminResult>> Handle(GetAllGroupForAdmin request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var groupList = await _context.GroupFpts
                .AsNoTracking()
                .Where(x => x.IsDelete != true)
                .ToListAsync();

            var allGroupList = new List<GetAllGroup>();

            foreach (var groups in groupList)
            {
                var group = new GetAllGroup();
                group.GroupId = groups.GroupId;
                group.GroupName = groups.GroupName;
                group.CoverImage = groups.CoverImage;
                group.NumberOfMember = await _context.GroupMembers.CountAsync(x => x.GroupId == groups.GroupId);
                group.CreatedAt = groups.CreatedDate;

                allGroupList.Add(group);
            }
            
            var result = new GetAllGroupForAdminResult
            {
                result = allGroupList.OrderByDescending(x => x.CreatedAt)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)allGroupList.Count() / request.PageSize),
            };

            return Result<GetAllGroupForAdminResult>.Success(result);
        }
    }
}
