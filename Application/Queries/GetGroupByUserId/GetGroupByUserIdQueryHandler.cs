using Application.DTO.GroupDTO;
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

namespace Application.Queries.GetGroupByUserId
{
    public class GetGroupByUserIdQueryHandler : IQueryHandler<GetGroupByUserIdQuery, GetGroupByUserIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetGroupByUserIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetGroupByUserIdQueryResult>> Handle(GetGroupByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var listgroup = await _context.GroupMembers.Include(x => x.Group).Where(x => x.UserId == request.UserId && x.IsJoined == true).ToListAsync();
            var listrole = await _context.GroupRoles.ToListAsync();
            var result = new GetGroupByUserIdQueryResult();
            if(listgroup != null)
            {
                foreach (var group in listgroup)
                {
                    if(group.Group.IsDelete == false)
                    {
                        if ((group.GroupRoleId == (listrole.FirstOrDefault(x => x.GroupRoleName.Equals("Admin")).GroupRoleId))
                       || (group.GroupRoleId == (listrole.FirstOrDefault(x => x.GroupRoleName.Equals("Censor")).GroupRoleId)))
                        {
                            var gr = new GetGroupByUserIdDTO
                            {
                                GroupId = group.GroupId,
                                GroupName = group.Group.GroupName,
                                CoverImage = group.Group.CoverImage
                            };
                            result.ListGroupAdmin.Add(gr);
                        }
                        else
                        {
                            var gr = new GetGroupByUserIdDTO
                            {
                                GroupId = group.GroupId,
                                GroupName = group.Group.GroupName,
                                CoverImage = group.Group.CoverImage
                            };
                            result.ListGroupMember.Add(gr);
                        }
                    }
                   
                }
            }
            return Result<GetGroupByUserIdQueryResult>.Success(result);
        }
    }
}
