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
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateRoleMember
{
    public class UpdateRoleMemberCommandHandler : ICommandHandler<UpdateRoleMemberCommand, UpdateRoleMemberCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;


        public UpdateRoleMemberCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<UpdateRoleMemberCommandResult>> Handle(UpdateRoleMemberCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new UpdateRoleMemberCommandResult();
            var member = await _querycontext.GroupMembers.Include(x=>x.GroupRole)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId);
            if(member != null)
            {
                if(request.Action == 1)
                {
                    if (member.GroupRole.GroupRoleName.Equals("Admin"))
                    {
                        var newrole = await _querycontext.GroupMembers
                            .FirstOrDefaultAsync(x => x.UserId == request.MemberId && x.GroupId == request.GroupId && x.IsJoined==true);
                         if(newrole == null)
                         {
                            throw new ErrorException(StatusCodeEnum.GR16_User_Not_Exist_In_Group);
                         }
                        var role = new Domain.CommandModels.GroupMember { 
                            GroupId = request.GroupId,
                            UserId = request.MemberId,
                            GroupRoleId = (Guid)request.GroupRoleId,
                            InvatedBy = newrole.InvatedBy,
                            IsJoined = newrole.IsJoined,
                            IsInvated = newrole.IsInvated,
                            JoinedDate = newrole.JoinedDate,       
                        };
                        result.IsDelete = false;
                        result.UpdateRole = true;
                        result.Message = "Update Role Success!";
                        _context.GroupMembers.Update(role);
                    }
                    else
                    {
                        throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
                    }
                }
                else
                {
                    if (member.GroupRole.GroupRoleName.Equals("Admin"))
                    {
                        var newrole = await _querycontext.GroupMembers
                            .FirstOrDefaultAsync(x => x.UserId == request.MemberId && x.GroupId == request.GroupId && x.IsJoined == true);
                        if (newrole == null)
                        {
                            throw new ErrorException(StatusCodeEnum.GR16_User_Not_Exist_In_Group);
                        }
                        var role = new Domain.CommandModels.GroupMember
                        {
                            GroupId = request.GroupId,
                            UserId = request.MemberId,
                            GroupRoleId = newrole.GroupRoleId,
                            InvatedBy = newrole.InvatedBy,
                            IsJoined = newrole.IsJoined,
                            IsInvated = newrole.IsInvated,
                            JoinedDate = newrole.JoinedDate,
                        };
                        result.IsDelete = true;
                        result.UpdateRole = false;
                        result.Message = "Remove User Success!";
                        _context.GroupMembers.Remove(role);
                    }
                    else if (member.GroupRole.GroupRoleName.Equals("Censor"))
                    {
                        var newrole = await _querycontext.GroupMembers.Include(x=>x.GroupRole)
                            .FirstOrDefaultAsync(x => x.UserId == request.MemberId && x.GroupId == request.GroupId && x.IsJoined == true);
                        if(newrole.GroupRole.GroupRoleName.Equals("Admin") || newrole.GroupRole.GroupRoleName.Equals("Censor"))
                        {
                            throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
                        }
                        var role = new Domain.CommandModels.GroupMember
                        {
                            GroupId = request.GroupId,
                            UserId = request.MemberId,
                            GroupRoleId = newrole.GroupRoleId,
                            InvatedBy = newrole.InvatedBy,
                            IsJoined = newrole.IsJoined,
                            IsInvated = newrole.IsInvated,
                            JoinedDate = newrole.JoinedDate,
                        };
                        result.IsDelete = true;
                        result.UpdateRole = false;
                        result.Message = "Remove User Success!";
                        _context.GroupMembers.Remove(role);
                    }
                    else
                    {
                        throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
                    }
                }

                await _context.SaveChangesAsync();
            }
            return Result<UpdateRoleMemberCommandResult>.Success(result);
        }
    }
}
