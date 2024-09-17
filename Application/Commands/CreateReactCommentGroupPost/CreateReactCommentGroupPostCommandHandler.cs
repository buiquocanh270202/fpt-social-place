using Application.Commands.CreateNewReact;
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

namespace Application.Commands.CreateReactCommentGroupPost
{
    public class CreateReactCommentGroupPostCommandHandler : ICommandHandler<CreateReactCommentGroupPostCommand, CreateReactCommentGroupPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactCommentGroupPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactCommentGroupPostCommandResult>> Handle(CreateReactCommentGroupPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            // Lấy thông tin comment
            var comment = await _querycontext.CommentGroupPosts.FirstOrDefaultAsync(c => c.CommentGroupPostId == request.CommentGroupPostId, cancellationToken);
            if (comment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // 1. Kiểm tra phản ứng (reaction) hiện có cho comment
            var existingReact = await _querycontext.ReactGroupCommentPosts
                .FirstOrDefaultAsync(r =>
                    r.GroupPostId == request.GroupPostId &&
                    r.CommentGroupPostId == request.CommentGroupPostId &&
                    r.UserId == request.UserId,
                    cancellationToken);

            Domain.CommandModels.ReactGroupCommentPost ReactComments = new();

            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Nếu cùng loại reaction, xóa phản ứng
                    var commandReact = new Domain.CommandModels.ReactGroupCommentPost
                    {
                        ReactGroupCommentPostId = existingReact.ReactGroupCommentPostId,
                        GroupPostId = existingReact.GroupPostId,
                        CommentGroupPostId = existingReact.CommentGroupPostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate,
                    };
                    _context.ReactGroupCommentPosts.Remove(commandReact);
                }
                else
                {
                    // Nếu khác loại, cập nhật loại reaction và thời gian
                    var commandReact = new Domain.CommandModels.ReactGroupCommentPost
                    {
                        ReactGroupCommentPostId = existingReact.ReactGroupCommentPostId,
                        GroupPostId = existingReact.GroupPostId,
                        CommentGroupPostId = existingReact.CommentGroupPostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _context.ReactGroupCommentPosts.Update(commandReact);
                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                ReactComments = new Domain.CommandModels.ReactGroupCommentPost
                {
                    ReactGroupCommentPostId = _helper.GenerateNewGuid(),
                    GroupPostId = request.GroupPostId,
                    CommentGroupPostId = request.CommentGroupPostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactGroupCommentPosts.AddAsync(ReactComments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // 5. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactCommentGroupPostCommandResult>(existingReact)
                : _mapper.Map<CreateReactCommentGroupPostCommandResult>(ReactComments);


            return Result<CreateReactCommentGroupPostCommandResult>.Success(result);
        }

    }
}
