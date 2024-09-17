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

namespace Application.Commands.CreateGroupType
{
    public class CreateGroupTypeCommandHandler : ICommandHandler<CreateGroupTypeCommand, CreateGroupTypeCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateGroupTypeCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateGroupTypeCommandResult>> Handle(CreateGroupTypeCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var grouptype = await _querycontext.GroupTypes.FirstOrDefaultAsync(x => x.GroupTypeName.Equals(request.GroupTypeName));
            if (grouptype != null)
            {
                throw new ErrorException(StatusCodeEnum.GR04_Group_Type_Existed);
            }
            var newgrouptype = new Domain.CommandModels.GroupType
            {
                GroupTypeId = _helper.GenerateNewGuid(),
                GroupTypeName = request.GroupTypeName,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };
            await _context.GroupTypes.AddAsync(newgrouptype);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateGroupTypeCommandResult>(newgrouptype);
            return Result<CreateGroupTypeCommandResult>.Success(result);
        }
    }
}
