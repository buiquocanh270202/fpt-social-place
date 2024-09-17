using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.RequestJoinGroupStatus
{
    public class RequestJoinStatusCommandHandler : ICommandHandler<RequestJoinStatusCommand, RequestJoinStatusCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;


        public RequestJoinStatusCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<RequestJoinStatusCommandResult>> Handle(RequestJoinStatusCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var admin = await _querycontext.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleName.Equals("Admin"));
            var censor = await _querycontext.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleName.Equals("Censor"));
            var member = await _querycontext.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleName.Equals("Member"));
            var getrole = await _querycontext.GroupMembers.FirstOrDefaultAsync(x => x.GroupId == request.GroupId && x.UserId == request.ManagerId);
            var memjoin = await _querycontext.GroupMembers.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId);
            var role = await _querycontext.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleId == getrole.GroupRoleId);
            if (role == null)
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }
            if (role.GroupRoleName.Equals("Member"))
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }
            var result = new RequestJoinStatusCommandResult();
            if (request.IsJoin)
            {
                var newmemjoin = new Domain.CommandModels.GroupMember
                {
                    GroupId = memjoin.GroupId,
                    GroupRoleId = memjoin.GroupRoleId,
                    UserId = memjoin.UserId,
                    IsJoined = true,
                    IsInvated = memjoin.IsInvated,
                    InvatedBy = memjoin.InvatedBy,
                    JoinedDate = memjoin.JoinedDate
                };
                _context.GroupMembers.Update(newmemjoin);
            }
            else
            {
                var joinrequest = await _querycontext.GroupMembers
                    .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId && x.IsJoined ==false);
                if(joinrequest != null)
                {
                    var newmemjoin = new Domain.CommandModels.GroupMember
                    {
                        GroupId = memjoin.GroupId,
                        GroupRoleId = memjoin.GroupRoleId,
                        UserId = memjoin.UserId,
                        IsJoined = false,
                        IsInvated = memjoin.IsInvated,
                        InvatedBy = memjoin.InvatedBy,
                        JoinedDate = memjoin.JoinedDate
                    };
                    _context.GroupMembers.Remove(newmemjoin);
                }
            }
            await _context.SaveChangesAsync();
            result.Message = "Accept Join!";
            result.IsJoin = request.IsJoin;
            result.UserId = memjoin.UserId;

            return Result<RequestJoinStatusCommandResult>.Success(result);
        }
    }
}
