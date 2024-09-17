using Application.Commands.CreateInterest;
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

namespace Application.Commands.CreateReactGroupPost
{
    public class CreateReactGroupPostCommandHandler : ICommandHandler<CreateReactGroupPostCommand, CreateReactGroupPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactGroupPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _queryContext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactGroupPostCommandResult>> Handle(CreateReactGroupPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // 1. Check for Existing Reaction
            var existingReact = await _queryContext.ReactGroupPosts
                .FirstOrDefaultAsync(r => r.GroupPostId == request.GroupPostId && r.UserId == request.UserId, cancellationToken);
            var postReactCount = await _queryContext.GroupPostReactCounts
                .FirstOrDefaultAsync(prc => prc.GroupPostId == request.GroupPostId);

            Domain.CommandModels.ReactGroupPost reactPost = new Domain.CommandModels.ReactGroupPost();
            if (existingReact != null)
            {
                // 2. Handle Existing Reaction (Update or Remove)
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Group clicked the same react type again -> Remove it
                    var commandReact = new Domain.CommandModels.ReactGroupPost
                    {
                        ReactGroupPostId = existingReact.ReactGroupPostId,
                        GroupPostId = existingReact.GroupPostId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate,
                    };
                    _context.ReactGroupPosts.Remove(commandReact);
                    if (postReactCount != null)
                    {
                        postReactCount.ReactCount--;

                        if (postReactCount.ReactCount < 0)
                        {
                            postReactCount.ReactCount = 0; 
                        }
                    }
                }
                else
                {
                    // Group changed react type -> Update
                    var commandReact = new Domain.CommandModels.ReactGroupPost
                    {
                        ReactGroupPostId = existingReact.ReactGroupPostId,
                        GroupPostId = existingReact.GroupPostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _context.ReactGroupPosts.Update(commandReact);
                }
            }
            else
            {
                // 3. Create New Reaction
                reactPost = new Domain.CommandModels.ReactGroupPost
                {
                    ReactGroupPostId = _helper.GenerateNewGuid(),
                    GroupPostId = request.GroupPostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactGroupPosts.AddAsync(reactPost);
                if (postReactCount != null)
                {
                    postReactCount.ReactCount++;
                }
            }

            if (postReactCount != null)
            {
                var prc = new Domain.CommandModels.GroupPostReactCount
                {
                    GroupPostReactCountId = postReactCount.GroupPostReactCountId,
                    GroupPostId = postReactCount.GroupPostId,
                    GroupPostPhotoId = postReactCount.GroupPostPhotoId,
                    ReactCount = postReactCount.ReactCount,
                    CommentCount = postReactCount.CommentCount,
                    ShareCount = postReactCount.ShareCount,
                };
                _context.GroupPostReactCounts.Update(prc);
            }
            
            await _context.SaveChangesAsync();

            // 4. Return Result (Consider Adjustments)
            var result = existingReact != null
                ? _mapper.Map<CreateReactGroupPostCommandResult>(existingReact) // If updated/removed
                : _mapper.Map<CreateReactGroupPostCommandResult>(reactPost);   // If newly created

            return Result<CreateReactGroupPostCommandResult>.Success(result);
        }

    }
}
