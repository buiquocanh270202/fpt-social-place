using Application.Commands.CreateReactUserPost;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactForGroupSharePost
{
    public class CreateReactForGroupSharePostCommandHandler : ICommandHandler<CreateReactForGroupSharePostCommand, CreateReactForGroupSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactForGroupSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext queryContext, IMapper mapper)
        {
            _context = context;
            _queryContext = queryContext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateReactForGroupSharePostCommandResult>> Handle(CreateReactForGroupSharePostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // 1. Check for Existing Reaction
            var existingReact = await _queryContext.ReactGroupSharePosts
                .FirstOrDefaultAsync(r => r.GroupSharePostId == request.GroupSharePostId && r.UserId == request.UserId, cancellationToken);
            //var postReactCount = await _context.PostReactCounts
            //    .FirstOrDefaultAsync(prc => prc.UserPostId == request.UserPostId);

            Domain.CommandModels.ReactGroupSharePost reactPost = new Domain.CommandModels.ReactGroupSharePost();
            if (existingReact != null)
            {
                // 2. Handle Existing Reaction (Update or Remove)
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // User clicked the same react type again -> Remove it
                    var commandReact = new Domain.CommandModels.ReactGroupSharePost
                    {
                        ReactGroupSharePostId = existingReact.ReactGroupSharePostId,
                        GroupSharePostId = existingReact.GroupSharePostId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreateDate = existingReact.CreateDate,
                    };
                    _context.ReactGroupSharePosts.Remove(commandReact);
                    
                }
                else
                {
                    // User changed react type -> Update
                    var commandReact = new Domain.CommandModels.ReactGroupSharePost
                    {
                        ReactGroupSharePostId = existingReact.ReactGroupSharePostId,
                        GroupSharePostId = existingReact.GroupSharePostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreateDate = DateTime.Now
                    };
                    _context.ReactGroupSharePosts.Update(commandReact);
                }
            }
            else
            {
                // 3. Create New Reaction
                reactPost = new Domain.CommandModels.ReactGroupSharePost
                {
                    ReactGroupSharePostId = _helper.GenerateNewGuid(),
                    GroupSharePostId = request.GroupSharePostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreateDate = DateTime.Now
                };

                await _context.ReactGroupSharePosts.AddAsync(reactPost);
            }

            await _context.SaveChangesAsync();

            // 4. Return Result (Consider Adjustments)
            var result = existingReact != null
                ? _mapper.Map<CreateReactForGroupSharePostCommandResult>(existingReact) // If updated/removed
                : _mapper.Map<CreateReactForGroupSharePostCommandResult>(reactPost);   // If newly created

            return Result<CreateReactForGroupSharePostCommandResult>.Success(result);
        }
    }
}
