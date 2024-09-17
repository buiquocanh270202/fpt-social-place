using Application.Commands.CreateReactUserPost;
using AutoMapper;
using Core.CQRS.Command;
using Core.CQRS;
using Core.Helper;
using Domain.CommandModels;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Commands.CreateNewReact
{
    public class CreateNewReactCommandHandler : ICommandHandler<CreateNewReactCommand, CreateNewReactCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateNewReactCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateNewReactCommandResult>> Handle(CreateNewReactCommand request, CancellationToken cancellationToken)
        {

            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            Domain.CommandModels.ReactType reactType = new Domain.CommandModels.ReactType
            {
                ReactTypeId = _helper.GenerateNewGuid(),
                ReactTypeName = request.ReactTypeName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _context.ReactTypes.AddAsync(reactType);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateNewReactCommandResult>(reactType);
            return Result<CreateNewReactCommandResult>.Success(result);
        }
    }
}
