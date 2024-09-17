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

namespace Application.Commands.CancleRequestJoinToGroup
{
    public class CancleRequestJoinCommandHandler : ICommandHandler<CancleRequestJoinCommand, CancleRequestJoinCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CancleRequestJoinCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CancleRequestJoinCommandResult>> Handle(CancleRequestJoinCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var member = await _querycontext.GroupMembers.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId);
            if(member.IsJoined == true || member == null)
            {
                throw new ErrorException(StatusCodeEnum.GR14_Can_Not_Cancel_Request);

            }

            var canceljoin = new Domain.CommandModels.GroupMember
            {
                GroupId = member.GroupId,
                UserId = member.UserId,
                IsJoined = member.IsJoined,
                JoinedDate = member.JoinedDate,
                GroupRoleId = member.GroupRoleId,
                IsInvated = member.IsInvated,
                InvatedBy = member.InvatedBy
            };
            _context.GroupMembers.Remove(canceljoin);
            await _context.SaveChangesAsync();
            var result = new CancleRequestJoinCommandResult {
                Message = "Cancel Request Success!",
                Calcel = true
            
            };

            return Result<CancleRequestJoinCommandResult>.Success(result);
        }
    }
}
