using Application.Queries.GetUserPost;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserRelationships
{
    public class GetUserRelationshipHandler : IQueryHandler<GetUserRelationshipQuery, GetUserRelationshipResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetUserRelationshipHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetUserRelationshipResult>> Handle(GetUserRelationshipQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request.UserId == null)
            {
                return Result<GetUserRelationshipResult>.Failure("UserId is required.");
            }

            var userRelationship = await _context.UserRelationships
                .Include(c => c.Relationship)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (userRelationship == null)
            {
                throw new ErrorException(StatusCodeEnum.UR01_NOT_FOUND);
            }

            /*if (userRelationship.IsActive == false)
            {
                throw new ErrorException(StatusCodeEnum.U02_Lock_User);
            }*/

            var result = _mapper.Map<GetUserRelationshipResult>(userRelationship);
            return Result<GetUserRelationshipResult>.Success(result);
        }
    }
}
