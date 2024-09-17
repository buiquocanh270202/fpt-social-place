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
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactGroupVideoPost
{
    public class CreateReactGroupVideoPostCommandHandler : ICommandHandler<CreateReactGroupVideoPostCommand, CreateReactGroupVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactGroupVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _queryContext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactGroupVideoPostCommandResult>> Handle(CreateReactGroupVideoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // 1. Kiểm tra phản ứng hiện có
            var existingReact = await _queryContext.ReactGroupVideoPosts
                .FirstOrDefaultAsync(r => r.GroupPostVideoId == request.GroupPostVideoId && r.UserId == request.UserId, cancellationToken);
            var postReactCount = await _queryContext.GroupPostReactCounts
                .FirstOrDefaultAsync(prc => prc.GroupPostVideoId == request.GroupPostVideoId);
            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    var commandReact = new Domain.CommandModels.ReactGroupVideoPost
                    {
                        ReactGroupVideoPostId = existingReact.ReactGroupVideoPostId,
                        GroupPostVideoId = existingReact.GroupPostVideoId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate,
                    };
                    _context.ReactGroupVideoPosts.Remove(commandReact);
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
                    // Khác loại phản ứng -> Cập nhật
                    var commandReact = new Domain.CommandModels.ReactGroupVideoPost
                    {
                        ReactGroupVideoPostId = existingReact.ReactGroupVideoPostId,
                        GroupPostVideoId = existingReact.GroupPostVideoId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _context.ReactGroupVideoPosts.Update(commandReact);
                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                Domain.CommandModels.ReactGroupVideoPost reactPost = new Domain.CommandModels.ReactGroupVideoPost
                {
                    ReactGroupVideoPostId = _helper.GenerateNewGuid(),
                    GroupPostVideoId = request.GroupPostVideoId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactGroupVideoPosts.AddAsync(reactPost);
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

            // 4. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactGroupVideoPostCommandResult>(existingReact) // Nếu cập nhật/xóa
                : _mapper.Map<CreateReactGroupVideoPostCommandResult>(await _context.ReactGroupVideoPosts.FirstOrDefaultAsync(r => r.GroupPostVideoId == request.GroupPostVideoId && r.UserId == request.UserId, cancellationToken)); // Nếu mới tạo

            return Result<CreateReactGroupVideoPostCommandResult>.Success(result);
        }
    }
}
