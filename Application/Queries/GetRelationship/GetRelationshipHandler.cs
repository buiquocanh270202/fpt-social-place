using Application.Queries.GetGender;
using Application.Queries.GetUserPost;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetRelationship
{
    public class GetRelationshipHandler : IQueryHandler<GetRelationshipQuery, List<GetRelationshipResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetRelationshipHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetRelationshipResult>>> Handle(GetRelationshipQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var relationships = await _context.Relationships.ToListAsync();
            List<GetRelationshipResult> result = new List<GetRelationshipResult>();
            if (relationships != null)
            {
                foreach (var gen in relationships)
                {
                    var mapgender = _mapper.Map<GetRelationshipResult>(gen);
                    result.Add(mapgender);
                }
            }

            return Result<List<GetRelationshipResult>>.Success(result);
        }
    }

}
