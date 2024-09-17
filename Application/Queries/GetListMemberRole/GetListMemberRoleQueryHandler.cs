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

namespace Application.Queries.GetListMemberRole
{
    public class GetListMemberRoleQueryHandler : IQueryHandler<GetListMemberRoleQuery, GetListMemberRoleQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetListMemberRoleQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetListMemberRoleQueryResult>> Handle(GetListMemberRoleQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new GetListMemberRoleQueryResult();
       
            var joined = await _context.GroupMembers
                .Include(x=>x.GroupRole)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId);
            var member = await _context.GroupMembers
              .Where(x => x.GroupId == request.GroupId && x.IsJoined == true)
              .Include(x => x.GroupRole)
              .Include(x => x.User)
              .ThenInclude(x => x.AvataPhotos)
              .ToListAsync();

            if (joined.GroupRole.GroupRoleName.Equals("Admin"))
            {

                if (member?.Count > 0)
                {
                    foreach (var item in member)
                    {
                        var mem = new GroupMemberDTO
                        {
                            UserId = item.UserId,
                            GroupId = request.GroupId,
                            MemberName = item.User.FirstName + " " + item.User.LastName,
                            Avata = item.User.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl,
                            GroupRoleId = item.GroupRoleId,
                            GroupRoleName = item.GroupRole.GroupRoleName
                        };
                        if (item.GroupRole.GroupRoleName.Equals("Admin"))
                        { 
                            result.GroupAdmin.Add(mem);
                        }
                        if (item.GroupRole.GroupRoleName.Equals("Censor"))
                        {    
                            result.GroupMangager.Add(mem);
                        }
                        if (item.GroupRole.GroupRoleName.Equals("Member"))
                        {
                            result.GroupMember.Add(mem);
                        }
                    }
                }
            }
            else if(joined.GroupRole.GroupRoleName.Equals("Censor"))
            {
                if (member?.Count > 0)
                {
                    foreach (var item in member)
                    {
                        var mem = new GroupMemberDTO
                        {
                            UserId = item.UserId,
                            GroupId = request.GroupId,
                            MemberName = item.User.FirstName + " " + item.User.LastName,
                            Avata = item.User.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl,
                            GroupRoleId = item.GroupRoleId,
                            GroupRoleName = item.GroupRole.GroupRoleName
                        };
                        if (item.GroupRole.GroupRoleName.Equals("Admin"))
                        {
                            result.GroupAdmin.Add(mem);
                        }
                        if (item.GroupRole.GroupRoleName.Equals("Censor"))
                        {
                            result.GroupMangager.Add(mem);
                        }
                        if (item.GroupRole.GroupRoleName.Equals("Member"))
                        {
                            result.GroupMember.Add(mem);
                        }
                    }
                }
            }
            else
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }

            return Result<GetListMemberRoleQueryResult>.Success(result);
        }
    }
}
