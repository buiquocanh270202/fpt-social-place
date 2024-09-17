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

namespace Application.Commands.InvatedJoinStatus
{
    public class InvatedJoinStatusCommandHandler : ICommandHandler<InvatedJoinStatusCommand, InvatedJoinStatusCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;


        public InvatedJoinStatusCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<InvatedJoinStatusCommandResult>> Handle(InvatedJoinStatusCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new InvatedJoinStatusCommandResult();
            var invated = await _querycontext.GroupMembers
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.InvatedBy == request.InvatedBy && x.GroupId == request.GroupId);
            if(invated != null)
            {
                if(request.IsAccept == true)
                {
                    var accept = new Domain.CommandModels.GroupMember { 
                        GroupId = request.GroupId,
                        UserId = invated.UserId,
                        GroupRoleId = invated.GroupRoleId,
                        InvatedBy = invated.InvatedBy,
                        IsJoined = true,
                        IsInvated = true,
                        JoinedDate = DateTime.Now
                    };
                    result.Message = "Accept to join!";
                    _context.GroupMembers.Update(accept);
                }
                else
                {
                    var reject = new Domain.CommandModels.GroupMember
                    {
                        GroupId = request.GroupId,
                        UserId = invated.UserId,
                        GroupRoleId = invated.GroupRoleId,
                        InvatedBy = invated.InvatedBy,
                        IsJoined = invated.IsJoined,
                        IsInvated = invated.IsInvated,
                        JoinedDate = invated.JoinedDate
                    };

                    result.Message = "Reject the Invated!";
                    _context.GroupMembers.Remove(reject);
                }
                result.IsAccept = request.IsAccept;
                await _context.SaveChangesAsync();
            }
            return Result<InvatedJoinStatusCommandResult>.Success(result);
        }
    }
}
