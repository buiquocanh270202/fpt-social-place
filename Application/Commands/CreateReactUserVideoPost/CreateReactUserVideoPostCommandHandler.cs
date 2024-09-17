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

namespace Application.Commands.CreateReactUserVideoPost
{
    public class CreateReactUserVideoPostCommandHandler : ICommandHandler<CreateReactUserVideoPostCommand, CreateReactUserVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactUserVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _queryContext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactUserVideoPostCommandResult>> Handle(CreateReactUserVideoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // 1. Kiểm tra phản ứng hiện có
            var existingReact = await _queryContext.ReactVideoPosts
                .FirstOrDefaultAsync(r => r.UserPostVideoId == request.UserPostVideoId && r.UserId == request.UserId, cancellationToken);
            var postReactCount = await _queryContext.PostReactCounts
                .FirstOrDefaultAsync(prc => prc.UserPostVideoId == request.UserPostVideoId);
            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    var commandReact = new Domain.CommandModels.ReactVideoPost
                    {
                        ReactVideoPostId = existingReact.ReactVideoPostId,
                        UserPostVideoId = existingReact.UserPostVideoId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate,
                    };
                    _context.ReactVideoPosts.Remove(commandReact);
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
                    var commandReact = new Domain.CommandModels.ReactVideoPost
                    {
                        ReactVideoPostId = existingReact.ReactVideoPostId,
                        UserPostVideoId = existingReact.UserPostVideoId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _queryContext.ReactVideoPosts.Update(existingReact);
                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                Domain.CommandModels.ReactVideoPost reactPost = new Domain.CommandModels.ReactVideoPost
                {
                    ReactVideoPostId = _helper.GenerateNewGuid(),
                    UserPostVideoId = request.UserPostVideoId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactVideoPosts.AddAsync(reactPost);
                if (postReactCount != null)
                {
                    postReactCount.ReactCount++;
                }
            }
            if (postReactCount != null)
            {
                var prc = new Domain.CommandModels.PostReactCount
                {
                    PostReactCountId = postReactCount.PostReactCountId,
                    UserPostId = postReactCount.UserPostId,
                    UserPostPhotoId = postReactCount.UserPostPhotoId,
                    UserPostVideoId = postReactCount.UserPostVideoId,
                    ReactCount = postReactCount.ReactCount,
                    CommentCount = postReactCount.CommentCount,
                    ShareCount = postReactCount.ShareCount,
                    CreateAt = postReactCount.CreateAt,
                    UpdateAt = DateTime.Now,
                };
                _context.PostReactCounts.Update(prc);
            }

            await _context.SaveChangesAsync();

            // 4. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactUserVideoPostCommandResult>(existingReact) // Nếu cập nhật/xóa
                : _mapper.Map<CreateReactUserVideoPostCommandResult>(await _context.ReactVideoPosts.FirstOrDefaultAsync(r => r.UserPostVideoId == request.UserPostVideoId && r.UserId == request.UserId, cancellationToken)); // Nếu mới tạo

            return Result<CreateReactUserVideoPostCommandResult>.Success(result);
        }
    }
}
