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

namespace Application.Commands.CreateAdminProfile
{
    public class CreateAdminProfileCommandHandler : ICommandHandler<CreateAdminProfileCommand, CreateAdminProfileCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateAdminProfileCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateAdminProfileCommandResult>> Handle(CreateAdminProfileCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request.Email == null)
            {
                throw new ErrorException(StatusCodeEnum.U04_Can_Not_Create);
            }

            var getuserbyemail = await _querycontext.AdminProfiles.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));

            if (getuserbyemail != null)
            {
                throw new ErrorException(StatusCodeEnum.U03_User_Exist);
            }
            var role = await _querycontext.Roles.FirstOrDefaultAsync(x => x.NameRole == request.RoleName);
            var result = new CreateAdminProfileCommandResult();

            if (!string.IsNullOrEmpty(request.FullName))
            {
                var admin = new Domain.CommandModels.AdminProfile
                {
                    AdminId = (Guid)request.AdminId,
                    RoleId = role.RoleId,
                    FullName = request.FullName,
                    Email = request.Email,
                    IsActive = true,
                    CreateDate = DateTime.Now
                };
                await _context.AdminProfiles.AddAsync(admin);
                await _context.SaveChangesAsync();
                result.Message = "Create Success!";
                result.FullName = request.FullName;
                result.Email = request.Email;
            }

            return Result<CreateAdminProfileCommandResult>.Success(result);

        }
    }
}
