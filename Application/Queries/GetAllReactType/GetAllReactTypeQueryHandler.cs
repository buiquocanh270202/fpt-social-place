using Application.Queries.GetGender;
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

namespace Application.Queries.GetAllReactType
{
    public class GetAllReactTypeQueryHandler : IQueryHandler<GetAllReactTypeQuery, List<GetAllReactTypeQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetAllReactTypeQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetAllReactTypeQueryResult>>> Handle(GetAllReactTypeQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var reactType = await _context.ReactTypes.ToListAsync();
            List<GetAllReactTypeQueryResult> result = new List<GetAllReactTypeQueryResult>();
            if (reactType != null)
            {
                foreach (var react in reactType)
                {
                    var mapReact = _mapper.Map<GetAllReactTypeQueryResult>(react);
                    result.Add(mapReact);
                }
            }

            return Result<List<GetAllReactTypeQueryResult>>.Success(result);
        }
    }
}
