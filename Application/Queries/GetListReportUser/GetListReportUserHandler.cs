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

namespace Application.Queries.GetListReportUser
{
    public class GetListReportUserHandler : IQueryHandler<GetListReportUserQuery, GetListReportUserResult>
    {
        private readonly fptforumQueryContext _context;

        public GetListReportUserHandler(fptforumQueryContext context) {
            _context = context;
        }
        public async Task<Result<GetListReportUserResult>> Handle(GetListReportUserQuery request, CancellationToken cancellationToken)
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
                .Include(x => x.User)
                .Include(x => x.ReportBy)
                .Where(x => x.UserId != null && x.UserId == request.UserId && x.Processing == true)
                .ToListAsync(cancellationToken);

            var reportList = reportListQuery.Select(x => new GetReportUser
            {
                ReportId = x.ReportProfileId,
                ReportTypeId = x.ReportTypeId,
                UserId = x.ReportBy.UserId,
                UserName = x.ReportBy.FullName,
                AvatarUrl = _context.AvataPhotos.Where(ap => ap.UserId == x.ReportBy.UserId && ap.IsUsed == true).Select(ap => ap.AvataPhotosUrl).FirstOrDefault(),
                ReportedUserId = x.User.UserId,
                ReportedUserName = x.User.FullName,
                ReportedAvatarUrl = _context.AvataPhotos.Where(ap => ap.UserId == x.User.UserId && ap.IsUsed == true).Select(ap => ap.AvataPhotosUrl).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
            }).ToList();

            var result = new GetListReportUserResult {
                result = reportList.OrderByDescending(x => x.CreatedDate)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)reportList.Count() / request.PageSize),
            };
            return Result<GetListReportUserResult>.Success(result);
        }
    }
}
