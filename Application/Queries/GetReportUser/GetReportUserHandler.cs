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

namespace Application.Queries.GetReportUser
{
    public class GetReportUserHandler : IQueryHandler<GetReportUserQuery, GetReportUserResult>
    {
        private readonly fptforumQueryContext _context;

        public GetReportUserHandler(fptforumQueryContext context)
        {
            _context = context;
        }
        public async Task<Result<GetReportUserResult>> Handle(GetReportUserQuery request, CancellationToken cancellationToken)
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
                .Include(x => x.User)
                .Include(x => x.ReportBy)
                .Where(x => x.UserId != null && x.Processing == true)
                .GroupBy(x => x.UserId)
                .Select(g => new GetReportUser
                {
                    UserReportedId = g.Key,
                    UserName = g.First().User.FullName,
                    AvatarUrl = _context.AvataPhotos
                        .Where(ap => ap.UserId == g.Key && ap.IsUsed == true)
                        .Select(ap => ap.AvataPhotosUrl)
                        .FirstOrDefault(),
                    NumberReporter = g.Count(),
                }).ToListAsync(cancellationToken);

            var result = new GetReportUserResult {
                result = reportList.OrderByDescending(x => x.NumberReporter)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)reportList.Count() / request.PageSize),
            };

            return Result<GetReportUserResult>.Success(result);
        }
    }
}
