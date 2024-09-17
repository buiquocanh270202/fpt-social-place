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

namespace Application.Queries.GetAllGroupRole
{
    public class GetAllGroupRoleQueryHandler : IQueryHandler<GetAllGroupRoleQuery, List<GetAllGroupRoleQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetAllGroupRoleQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetAllGroupRoleQueryResult>>> Handle(GetAllGroupRoleQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var reusult = new List<GetAllGroupRoleQueryResult>();   
            var grouprole = await _context.GroupRoles.ToListAsync();
            if (grouprole != null)
            {
                foreach(var gr in grouprole)
                {
                    var role = _mapper.Map<GetAllGroupRoleQueryResult>(gr);
                    reusult.Add(role);
                }
            }

            return Result<List<GetAllGroupRoleQueryResult>>.Success(reusult);
        }
    }
}
