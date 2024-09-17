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

namespace Application.Commands.CreateInterest
{
    public class CreateInterestCommandHandler : ICommandHandler<CreateInterestCommand, CreateInterestCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateInterestCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateInterestCommandResult>> Handle(CreateInterestCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var interest = await _querycontext.Interests.FirstOrDefaultAsync(x => x.InterestName.Equals(request.InterestName));
            if(interest != null)
            {
                throw new ErrorException(StatusCodeEnum.IT01_Interest_Existed);
            }

            var newinterest = new Domain.CommandModels.Interest {
                InterestId = _helper.GenerateNewGuid(),
                InterestName = request.InterestName,
                CreatedAt = DateTime.Now
            };

            await _context.Interests.AddAsync(newinterest);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateInterestCommandResult>(newinterest);
            return Result<CreateInterestCommandResult>.Success(result);
        }
    }
}
