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

namespace Application.Commands.CreateReactCommentUserPost
{
    public class CreateReactCommentUserPostCommandHandler : ICommandHandler<CreateReactCommentUserPostCommand, CreateReactCommentUserPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactCommentUserPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactCommentUserPostCommandResult>> Handle(CreateReactCommentUserPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var comment = await _querycontext.CommentPosts.FirstOrDefaultAsync(c => c.CommentId == request.CommentId, cancellationToken);
            if (comment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // 1. Kiểm tra phản ứng (reaction) hiện có cho comment
            var existingReact = await _querycontext.ReactComments
                .FirstOrDefaultAsync(r =>
                    r.UserPostId == request.UserPostId &&
                    r.CommentId == request.CommentId &&
                    r.UserId == request.UserId,
                    cancellationToken);

            Domain.CommandModels.ReactComment ReactComments = new();

            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Nếu cùng loại reaction, xóa phản ứng
                    var commandReact = new Domain.CommandModels.ReactComment
                    {
                        ReactCommentId = existingReact.ReactCommentId,
                        UserPostId = existingReact.UserPostId,
                        CommentId = existingReact.CommentId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate,
                    };
                    _context.ReactComments.Remove(commandReact);

                }
                else
                {
                    // Nếu khác loại, cập nhật loại reaction và thời gian
                    var commandReact = new Domain.CommandModels.ReactComment
                    {
                        ReactCommentId = existingReact.ReactCommentId,
                        UserPostId = existingReact.UserPostId,
                        CommentId = existingReact.CommentId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _context.ReactComments.Update(commandReact);

                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                ReactComments = new Domain.CommandModels.ReactComment
                {
                    ReactCommentId = _helper.GenerateNewGuid(),
                    UserPostId = request.UserPostId,
                    CommentId = request.CommentId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactComments.AddAsync(ReactComments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);


            // 5. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactCommentUserPostCommandResult>(existingReact)
                : _mapper.Map<CreateReactCommentUserPostCommandResult>(ReactComments);


            return Result<CreateReactCommentUserPostCommandResult>.Success(result);
        }

    }
}
