using Application.Queries.GetReportUser;
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

namespace Application.Queries.GetReportGroup
{
    public class GetReportGroupHandler : IQueryHandler<GetReportGroupQuery, GetReportGroupResult>
    {
        private readonly fptforumQueryContext _context;

        public GetReportGroupHandler(fptforumQueryContext context)
        {
            _context = context;
        }
        public async Task<Result<GetReportGroupResult>> Handle(GetReportGroupQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reportList = await _context.ReportProfiles
                .Include(x => x.Group)
                .Include(x => x.ReportBy)
                .Where(x => x.GroupId != null && x.Processing == true)
                .GroupBy(x => x.GroupId)
                .Select(g => new GetReportGroup
                {
                    GroupReportedId = g.Key,
                    GroupName = g.First().Group.GroupName,
                    GroupCoverImage = g.First().Group.CoverImage,
                    NumberReporter = g.Count(),
                }).ToListAsync(cancellationToken);

            var result = new GetReportGroupResult
            {
                result = reportList.OrderByDescending(x => x.NumberReporter)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)reportList.Count() / request.PageSize),
            };

            return Result<GetReportGroupResult>.Success(result);
        }
    }
}
