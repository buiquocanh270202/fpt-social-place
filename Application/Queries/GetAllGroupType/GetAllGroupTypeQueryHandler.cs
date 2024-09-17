using Application.Queries.GetAllGroupTag;
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

namespace Application.Queries.GetAllGroupType
{
    public class GetAllGroupTypeQueryHandler : IQueryHandler<GetAllGroupTypeQuery, List<GetAllGroupTypeQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetAllGroupTypeQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetAllGroupTypeQueryResult>>> Handle(GetAllGroupTypeQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var grouptype = await _context.GroupTypes.ToListAsync();
            var result = new List<GetAllGroupTypeQueryResult>();
            if (grouptype != null)
            {
                foreach (var gt in grouptype)
                {
                    var tag = _mapper.Map<GetAllGroupTypeQueryResult>(gt);
                    result.Add(tag);
                }
            }
            return Result<List<GetAllGroupTypeQueryResult>>.Success(result);
        }
    }
}
