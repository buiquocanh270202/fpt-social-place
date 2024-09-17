using Application.Queries.GetGender;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserStatus
{
    public class GetUserStatusQueryHandler : IQueryHandler<GetUserStatusQuery, List<GetUserStatusQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetUserStatusQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetUserStatusQueryResult>>> Handle(GetUserStatusQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var status = await _context.UserStatuses.ToListAsync();
            List<GetUserStatusQueryResult> result = new List<GetUserStatusQueryResult>();
            if (status != null)
            {
                foreach (var st in status)
                {
                    var mapst = _mapper.Map<GetUserStatusQueryResult>(st);
                    result.Add(mapst);
                }
            }
            return Result<List<GetUserStatusQueryResult>>.Success(result);
        }
    }
}
