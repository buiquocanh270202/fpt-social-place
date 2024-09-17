using Application.DTO.GroupDTO;
using AutoMapper;
using AutoMapper.Execution;
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

namespace Application.Queries.GetListRequestjoinGroup
{
    public class GetListRequestJoinGroupQueryHandler : IQueryHandler<GetListRequestJoinGroupQuery, GetListRequestJoinGroupQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetListRequestJoinGroupQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetListRequestJoinGroupQueryResult>> Handle(GetListRequestJoinGroupQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var admin = await _context.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleName.Equals("Admin"));
            var censor = await _context.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleName.Equals("Censor"));
            var getrole = await _context.GroupMembers.FirstOrDefaultAsync(x => x.GroupId == request.GroupId && x.UserId == request.UserId);
            var groupmember = await _context.GroupMembers.Include(x=>x.User).ThenInclude(x=>x.AvataPhotos).Where(x => x.GroupId == request.GroupId && x.IsJoined == false).ToListAsync();
            var isjoin = await _context.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleId == getrole.GroupRoleId);
            if (isjoin == null)
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }
            if (isjoin.GroupRoleName.Equals("Member"))
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }

            var result = new GetListRequestJoinGroupQueryResult();
            if(groupmember != null)
            {
                foreach (var mem in groupmember)
                {
                    if(mem.InvatedBy == null || mem.IsInvated == true)
                    {
                        var memdto = new RequestJoinGroupDTO
                        {
                            UserId = mem.UserId,
                            UserName = mem.User.FirstName + " " + mem.User.LastName,

                        };
                        if (mem.User.AvataPhotos.Count > 0)
                        {
                            memdto.UserAvata = mem.User.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl;
                        }
                        else
                        {
                            memdto.UserAvata = null;
                        }
                        result.requestJoinGroups.Add(memdto);
                    }
                }
            }  
            return Result<GetListRequestJoinGroupQueryResult>.Success(result);
        }
    }
}
