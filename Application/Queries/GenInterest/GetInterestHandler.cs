using System;
using Application.Queries.GetGender;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GenInterest
{
	public class GetInterestHandler : IQueryHandler<GetInterestQuery, List<GetInterestResult>>
    {
		private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetInterestHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetInterestResult>>> Handle(GetInterestQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var interests = await _context.Interests.ToListAsync();
            List<GetInterestResult> result = new List<GetInterestResult>();
            if (interests != null)
            {
                foreach (var interest in interests)
                {
                    var mapgender = _mapper.Map<GetInterestResult>(interest);
                    result.Add(mapgender);
                }
            }

            return Result<List<GetInterestResult>>.Success(result);
        }
	}
}

