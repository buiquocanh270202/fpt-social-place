using Application.Commands.CreateGroupRole;
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

namespace Application.Commands.CreateGroupStatus
{
    public class CreateGroupStatusCommandHandler : ICommandHandler<CreateGroupStatusCommand, CreateGroupStatusCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateGroupStatusCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateGroupStatusCommandResult>> Handle(CreateGroupStatusCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var groupstatus = await _querycontext.GroupStatuses.FirstOrDefaultAsync(x => x.GroupStatusName.Equals(request.GroupStatusName));
            if (groupstatus != null)
            {
                throw new ErrorException(StatusCodeEnum.GR05_Group_Status_Existed);
            }
            var newgroupstatus = new Domain.CommandModels.GroupStatus
            {
                GroupStatusId = _helper.GenerateNewGuid(),
                GroupStatusName = request.GroupStatusName   ,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };
            await _context.GroupStatuses.AddAsync(newgroupstatus);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateGroupStatusCommandResult>(newgroupstatus);
            return Result<CreateGroupStatusCommandResult>.Success(result);
        }
    }
}
