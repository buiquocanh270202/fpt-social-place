using Application.Queries.GetAllGroupStatus;
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

namespace Application.Queries.GetGroupStatusForCreate
{
    public class GetGroupStatusForCreateQueryHandler : IQueryHandler<GetGroupStatusForCreateQuery, List<GetGroupStatusForCreateQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetGroupStatusForCreateQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetGroupStatusForCreateQueryResult>>> Handle(GetGroupStatusForCreateQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var status = await _context.GroupStatuses.ToListAsync();
            var result = new List<GetGroupStatusForCreateQueryResult>();
            if (status != null)
            {
                foreach (var st in status)
                {
                    if(st.GroupStatusName.Equals("Public") || st.GroupStatusName.Equals("Private"))
                    {
                        var mapstatus = _mapper.Map<GetGroupStatusForCreateQueryResult>(st);
                        result.Add(mapstatus);
                    }     
                }
            }

            return Result<List<GetGroupStatusForCreateQueryResult>>.Success(result);
        }
    }
}
