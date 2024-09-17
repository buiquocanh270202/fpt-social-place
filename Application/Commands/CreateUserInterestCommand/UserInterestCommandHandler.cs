using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Domain.Exceptions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.DTO.CreateUserDTO;
using Application.Commands.CreateUserInterest;
using Core.Helper;
using Domain.CommandModels;
using Domain.QueryModels;

namespace Application.Commands.CreateUserInterest
{
    public class UserInterestCommandHandler : ICommandHandler<UserInterestCommand, UserInterestCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public UserInterestCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<UserInterestCommandResult>> Handle(UserInterestCommand request, CancellationToken cancellationToken)
        {
            if(_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var user = _querycontext.UserProfiles.FirstOrDefault(x => x.UserId == request.UserId);
            var interest = _querycontext.Interests.FirstOrDefault(x => x.InterestId == request.InterestId);
            if(user == null || interest == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }
            var UserInterest = new Domain.CommandModels.UserInterest
            {
                UserInterestId = _helper.GenerateNewGuid(),
                InterestId = request.InterestId,
                UserId = request.UserId,
                UserStatusId =request.UserStatusId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            

            await _context.UserInterests.AddAsync(UserInterest);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<UserInterestCommandResult>(UserInterest);
            return Result<UserInterestCommandResult>.Success(result); 
        }
    }
}
