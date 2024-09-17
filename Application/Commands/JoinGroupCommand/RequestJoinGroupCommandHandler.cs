using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
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

namespace Application.Commands.JoinGroupCommand
{
    public class RequestJoinGroupCommandHandler : ICommandHandler<RequestJoinGroupCommand, RequestJoinGroupCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;


        public RequestJoinGroupCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
    
        }
        public async Task<Result<RequestJoinGroupCommandResult>> Handle(RequestJoinGroupCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var joined = await _querycontext.GroupMembers.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId);
            var grouprole = await _querycontext.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleName.Equals("Member"));
            /*var groupstatus = await _querycontext.GroupStatuses.FirstOrDefaultAsync(x=>x.GroupStatusName.Equals("Public"));
            var groupsetting = await _querycontext.GroupSettingUses.Include(x=>x.GroupSetting).ToListAsync();
            var checkjoin = groupsetting.FirstOrDefault(x => x.GroupSetting.GroupSettingName.Equals("Join Automatically"));*/

            var isAutoJoin = await _querycontext.GroupSettingUses
                .AsNoTracking()
                .Include(x => x.GroupSetting)
                .Include(x => x.GroupStatus)
                .AnyAsync(x => x.GroupId == request.GroupId && x.GroupSetting.GroupSettingName == "Join Automatically" && x.GroupStatus.GroupStatusName == "Public");

            if (joined != null)
            {
                if(joined.InvatedBy != null && joined.IsInvated == false)
                {
                    var clearinvate = new Domain.CommandModels.GroupMember
                    {
                        GroupId = joined.GroupId,
                        UserId = joined.UserId,
                        GroupRoleId = joined.GroupRoleId,
                        IsInvated = joined.IsInvated,
                        InvatedBy = joined.InvatedBy,
                        JoinedDate = joined.JoinedDate,
                        IsJoined = joined.IsJoined,
                    };
                    _context.GroupMembers.Remove(clearinvate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ErrorException(StatusCodeEnum.GR10_Group_Joined);
                }
            }

            var result = new RequestJoinGroupCommandResult();
            var requestJoin = new Domain.CommandModels.GroupMember
            {
                GroupId = request.GroupId,
                UserId = (Guid)request.UserId,
                GroupRoleId = grouprole.GroupRoleId,
                IsInvated = false,
                InvatedBy = null,
                JoinedDate = DateTime.Now,
                
            };
            if(isAutoJoin)
            {
                requestJoin.IsJoined = true;
                result.IsJoin = true;
                result.IsRequest = false;
            }
            else
            {
                requestJoin.IsJoined = false;
                result.IsRequest = true;
                result.IsJoin = false;
            }
            await _context.GroupMembers.AddAsync(requestJoin);
            await _context.SaveChangesAsync();
            result.Message = "Join to group success!";

            return Result<RequestJoinGroupCommandResult>.Success(result);
            
        }
    }
}
