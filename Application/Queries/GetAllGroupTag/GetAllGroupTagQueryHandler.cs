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

namespace Application.Queries.GetAllGroupTag
{
    public class GetAllGroupTagQueryHandler : IQueryHandler<GetAllGroupTagQuery, List<GetAllGroupTagQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetAllGroupTagQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetAllGroupTagQueryResult>>> Handle(GetAllGroupTagQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var grouptag = await _context.GroupTags.ToListAsync();
            var result = new List<GetAllGroupTagQueryResult>();
            if(grouptag != null)
            {
                foreach(var gt in grouptag)
                {
                    var tag = _mapper.Map<GetAllGroupTagQueryResult>(gt);
                    result.Add(tag);
                }
            }
            return Result<List<GetAllGroupTagQueryResult>>.Success(result);
        }
    }
}
