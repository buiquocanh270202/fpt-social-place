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

namespace Application.Commands.CreateRole
{
    public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, CreateRoleCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateRoleCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateRoleCommandResult>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var role = await _querycontext.Roles.FirstOrDefaultAsync(x => x.NameRole.Equals(request.NameRole));
            if(role != null)
            {
                throw new ErrorException(StatusCodeEnum.R01_Role_Existed);
            }

            var newrole = new Domain.CommandModels.Role { 
                RoleId = _helper.GenerateNewGuid(),
                NameRole = request.NameRole,
                CreatedAt = DateTime.Now,
            };

            await _context.Roles.AddAsync(newrole);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateRoleCommandResult>(newrole);
            return Result<CreateRoleCommandResult>.Success(result);
        }
    }
}
