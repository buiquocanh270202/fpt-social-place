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

namespace Application.Commands.CreateReactCommentUserPostVideo
{
    public class CreateReactCommentUserPostVideoCommandHandler : ICommandHandler<CreateReactCommentUserPostVideoCommand, CreateReactCommentUserPostVideoCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactCommentUserPostVideoCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactCommentUserPostVideoCommandResult>> Handle(CreateReactCommentUserPostVideoCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var comment = await _querycontext.CommentVideoPosts.FirstOrDefaultAsync(c => c.CommentVideoPostId == request.CommentVideoPostId, cancellationToken);
            if (comment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // 1. Kiểm tra phản ứng (reaction) hiện có cho comment
            var existingReact = await _querycontext.ReactVideoPostComments
                .FirstOrDefaultAsync(r =>
                    r.UserPostVideoId == request.UserPostVideoId &&
                    r.CommentVideoPostId == request.CommentVideoPostId &&
                    r.UserId == request.UserId,
                    cancellationToken);

            Domain.CommandModels.ReactVideoPostComment ReactComments = new();

            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Nếu cùng loại reaction, xóa phản ứng
                    var commandReact = new Domain.CommandModels.ReactVideoPostComment
                    {
                        ReactVideoPostCommentId = existingReact.ReactVideoPostCommentId,
                        UserPostVideoId = existingReact.UserPostVideoId,
                        CommentVideoPostId = existingReact.CommentVideoPostId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate,
                    };
                    _context.ReactVideoPostComments.Remove(commandReact);
                }
                else
                {
                    // Nếu khác loại, cập nhật loại reaction và thời gian
                    var commandReact = new Domain.CommandModels.ReactVideoPostComment
                    {
                        ReactVideoPostCommentId = existingReact.ReactVideoPostCommentId,
                        UserPostVideoId = existingReact.UserPostVideoId,
                        CommentVideoPostId = existingReact.CommentVideoPostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _context.ReactVideoPostComments.Update(commandReact);
                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                ReactComments = new Domain.CommandModels.ReactVideoPostComment
                {
                    ReactVideoPostCommentId = _helper.GenerateNewGuid(),
                    UserPostVideoId = request.UserPostVideoId,
                    CommentVideoPostId = request.CommentVideoPostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactVideoPostComments.AddAsync(ReactComments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // 5. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactCommentUserPostVideoCommandResult>(existingReact)
                : _mapper.Map<CreateReactCommentUserPostVideoCommandResult>(ReactComments);


            return Result<CreateReactCommentUserPostVideoCommandResult>.Success(result);
        }

    }
}
