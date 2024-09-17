using AutoMapper;
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

namespace Application.Queries.GetReportType
{
    public class GetReportTypeHandler : IQueryHandler<GetReportTypeQuery, List<GetReportTypeResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetReportTypeHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<GetReportTypeResult>>> Handle(GetReportTypeQuery request, CancellationToken cancellationToken)
        {
            if(_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if(request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reportType = await _context.ReportTypes.ToListAsync();

            var result = reportType.Select(x => new GetReportTypeResult {
                ReportTypeId = x.ReportTypeId,
                ReportTypeName = x.ReportTypeName,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();

            return Result<List<GetReportTypeResult>>.Success(result);
        }
    }
}
