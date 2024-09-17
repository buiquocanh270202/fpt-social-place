using Application.Queries.GetAllGroupSetting;
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

namespace Application.Queries.GetAllGroupStatus
{
    public class GetGroupStatusQueryHandler : IQueryHandler<GetGroupStatusQuery, List<GetGroupStatusQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetGroupStatusQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetGroupStatusQueryResult>>> Handle(GetGroupStatusQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var status = await _context.GroupStatuses.ToListAsync();
            var result = new List<GetGroupStatusQueryResult>();
            if (status != null)
            {
                foreach (var st in status)
                {
                    var mapstatus = _mapper.Map<GetGroupStatusQueryResult>(st);
                    result.Add(mapstatus);
                }
            }

            return Result<List<GetGroupStatusQueryResult>>.Success(result);
        }
    }
}
