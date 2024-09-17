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

namespace Application.Commands.CreateStatus
{
    public class CreateStatusCommandHandler : ICommandHandler<CreateStatusCommand, CreateStatusCommandResult>
    {

        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateStatusCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateStatusCommandResult>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var status = await _querycontext.UserStatuses.FirstOrDefaultAsync(x => x.StatusName.Equals(request.StatusName));
            if(status != null)
            {
                throw new ErrorException(StatusCodeEnum.ST02_Status_Existed);
            }

            var newstatus = new Domain.CommandModels.UserStatus {
                UserStatusId = _helper.GenerateNewGuid(),
                StatusName = request.StatusName,
                CreatedAt = DateTime.Now
                
            };
            await _context.UserStatuses.AddAsync(newstatus);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateStatusCommandResult>(newstatus);
            return Result<CreateStatusCommandResult>.Success(result);
        }
    }
}
