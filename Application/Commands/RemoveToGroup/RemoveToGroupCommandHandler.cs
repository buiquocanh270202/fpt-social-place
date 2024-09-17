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

namespace Application.Commands.RemoveToGroup
{
    public class RemoveToGroupCommandHandler : ICommandHandler<RemoveToGroupCommand, RemoveToGroupCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;


        public RemoveToGroupCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<RemoveToGroupCommandResult>> Handle(RemoveToGroupCommand request, CancellationToken cancellationToken)
        {

            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var joined = await _querycontext.GroupMembers.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId);
            if (joined == null)
            {
                throw new ErrorException(StatusCodeEnum.GR15_Can_Not_Out_Group);
            }
            if (joined.IsJoined == false)
            {
                throw new ErrorException(StatusCodeEnum.GR15_Can_Not_Out_Group);
            }

            var outgroup = new Domain.CommandModels.GroupMember {
                 UserId = joined.UserId,
                 GroupId = joined.GroupId,
                 IsJoined = joined.IsJoined,
                 GroupRoleId = joined.GroupRoleId,
                 IsInvated = joined.IsInvated,
                 InvatedBy = joined.InvatedBy,
                 JoinedDate = joined.JoinedDate
            };
            _context.GroupMembers.Remove(outgroup);
            await _context.SaveChangesAsync();
            var result = new RemoveToGroupCommandResult
            {
                Remove = true,
                Message = "Left the group is successfully!"
            };
            return Result<RemoveToGroupCommandResult>.Success(result);
        }
    }
}
