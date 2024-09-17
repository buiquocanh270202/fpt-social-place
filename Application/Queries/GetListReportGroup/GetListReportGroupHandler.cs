using Application.Queries.GetListReportUser;
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

namespace Application.Queries.GetListReportGroup
{
    public class GetListReportGroupHandler : IQueryHandler<GetListReportGroupQuery, GetListReportGroupResult>
    {
        private readonly fptforumQueryContext _context;

        public GetListReportGroupHandler(fptforumQueryContext context)
        {
            _context = context;
        }
        public async Task<Result<GetListReportGroupResult>> Handle(GetListReportGroupQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reportListQuery = await _context.ReportProfiles
                .Include(x => x.Group)
                .Include(x => x.ReportBy)
                .Where(x => x.GroupId == request.GroupId && x.Processing == true)
                .ToListAsync(cancellationToken);

            var reportList = reportListQuery.Select(x => new GetReportGroup
            {
                ReportId = x.ReportProfileId,
                ReportTypeId = x.ReportTypeId,
                UserId = x.ReportBy.UserId,
                UserName = x.ReportBy.FullName,
                AvatarUrl = _context.AvataPhotos.Where(ap => ap.UserId == x.ReportBy.UserId && ap.IsUsed == true).Select(ap => ap.AvataPhotosUrl).FirstOrDefault(),
                ReportedGroupId = x.Group.GroupId,
                ReportedGroupName = x.Group.GroupName,
                ReportedGroupCoverImage = x.Group.CoverImage,
                CreatedDate = x.CreatedDate,
            }).ToList();

            var result = new GetListReportGroupResult
            {
                result = reportList.OrderByDescending(x => x.CreatedDate)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)reportList.Count() / request.PageSize),
            };

            return Result<GetListReportGroupResult>.Success(result);
        }
    }
}
