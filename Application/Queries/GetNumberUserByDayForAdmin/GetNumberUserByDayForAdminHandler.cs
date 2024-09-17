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

namespace Application.Queries.GetNumberUserByDayForAdmin
{
    public class GetNumberUserByDayForAdminHandler : IQueryHandler<GetNumberUserByDayForAdmin, GetNumberUserByDayForAdminResult>
    {
        private readonly fptforumQueryContext _context;

        public GetNumberUserByDayForAdminHandler(fptforumQueryContext context)
        {
            _context = context;
        }
        public async Task<Result<GetNumberUserByDayForAdminResult>> Handle(GetNumberUserByDayForAdmin request, CancellationToken cancellationToken)
        {
            if(_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var numberUserByDayList = await _context.UserProfiles
                .AsNoTracking()
                .GroupBy(up => up.CreatedAt)
                .Select(g => new
                {
                    Date = g.Key,
                    UserCount = g.Count()
                })
                .ToListAsync();
            var allNumberUserByDay = new List<GetNumberUserByDay>();

            foreach (var item in numberUserByDayList)
            {
                var numberUserByDay = new GetNumberUserByDay
                {
                    Day = item.Date,
                    NumberUser = item.UserCount
                };

                allNumberUserByDay.Add(numberUserByDay);
            }

            var result = new GetNumberUserByDayForAdminResult
            {
                result = allNumberUserByDay.OrderByDescending(x => x.Day)
                                            .Skip((request.Page - 1) * request.PageSize)
                                            .Take(request.PageSize)
                                            .ToList(),
                totalPage = (int)Math.Ceiling((double)allNumberUserByDay.Count() / request.PageSize)
            };
            return Result<GetNumberUserByDayForAdminResult>.Success(result);
        }
    }
}
