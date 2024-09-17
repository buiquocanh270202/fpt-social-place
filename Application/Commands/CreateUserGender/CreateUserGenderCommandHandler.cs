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

namespace Application.Commands.CreateUserGender
{
    public class CreateUserGenderCommandHandler : ICommandHandler<CreateUserGenderCommand, CreateUserGenderCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateUserGenderCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateUserGenderCommandResult>> Handle(CreateUserGenderCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext ==  null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var user = await _querycontext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            var gender = await _querycontext.Genders.FirstOrDefaultAsync(x => x.GenderId == request.GenderId);
            if(user == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }
            if (gender == null)
            {
                throw new ErrorException(StatusCodeEnum.G02_Gender_Not_Found);
            }

            var usergender =  _mapper.Map<Domain.CommandModels.UserGender>(request);
            usergender.CreatedAt = DateTime.Now;
            usergender.UpdatedAt = DateTime.Now;
            await _context.UserGenders.AddAsync(usergender);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateUserGenderCommandResult>(usergender);
            return Result<CreateUserGenderCommandResult>.Success(result);
        }
    }
}
