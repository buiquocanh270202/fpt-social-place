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

namespace Application.Commands.CreateGroupRole
{
    public class CreateGroupRoleCommandHandler : ICommandHandler<CreateGroupRoleCommand, CreateGroupRoleCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateGroupRoleCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateGroupRoleCommandResult>> Handle(CreateGroupRoleCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var grouprole = await _querycontext.GroupRoles.FirstOrDefaultAsync(x => x.GroupRoleName.Equals(request.GroupRoleName));
            if(grouprole != null)
            {
                throw new ErrorException(StatusCodeEnum.GR01_Group_Role_Existed);
            }
            var newgrouprole = new Domain.CommandModels.GroupRole
            {
                GroupRoleId = _helper.GenerateNewGuid(),
                GroupRoleName = request.GroupRoleName,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };
            await _context.GroupRoles.AddAsync(newgrouprole);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateGroupRoleCommandResult>(newgrouprole);
            return Result<CreateGroupRoleCommandResult>.Success(result);
        }
    }
}
