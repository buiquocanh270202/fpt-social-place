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

namespace Application.Commands.CreateGroupTag
{
    public class CreateGroupTagCommandHandler : ICommandHandler<CreateGroupTagCommand, CreateGroupTagCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateGroupTagCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateGroupTagCommandResult>> Handle(CreateGroupTagCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var grouptag = await _querycontext.GroupTags.FirstOrDefaultAsync(x => x.TagName.Equals(request.TagName));
            if(grouptag != null)
            {
                throw new ErrorException(StatusCodeEnum.GR02_Group_Tag_Existed);
            }
            var newtag = new Domain.CommandModels.GroupTag
            {
                TagId = _helper.GenerateNewGuid(),
                TagName = request.TagName,
                CreatedAt = DateTime.Now,
                UpdateAt = null
            };
            await _context.GroupTags.AddAsync(newtag);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateGroupTagCommandResult>(newtag);
            return Result<CreateGroupTagCommandResult>.Success(result);
        }
    }
}
