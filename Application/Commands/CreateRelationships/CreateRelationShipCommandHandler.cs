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

namespace Application.Commands.CreateRelationships
{
    public class CreateRelationShipCommandHandler : ICommandHandler<CreateRelationShipCommand, CreateRelationShipCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateRelationShipCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateRelationShipCommandResult>> Handle(CreateRelationShipCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var relationship = await _querycontext.Relationships.FirstOrDefaultAsync(x => x.RelationshipName.Equals(request.RelationshipName));
            if(relationship != null)
            {
                throw new ErrorException(StatusCodeEnum.RL01_Relationship_Existed);
            }

            var newrelationship = new Domain.CommandModels.Relationship { 
                RelationshipId = _helper.GenerateNewGuid(),
                RelationshipName = request.RelationshipName,
                CreatedAt = DateTime.Now
            
            };
            await _context.Relationships.AddAsync(newrelationship);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateRelationShipCommandResult>(newrelationship);
            return Result<CreateRelationShipCommandResult>.Success(result);
        }
    }
}
