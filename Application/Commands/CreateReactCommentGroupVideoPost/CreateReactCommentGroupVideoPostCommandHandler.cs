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

namespace Application.Commands.CreateReactCommentGroupVideoPost
{
    public class CreateReactCommentGroupPostVideoCommandHandler : ICommandHandler<CreateReactCommentGroupPostVideoCommand, CreateReactCommentGroupPostVideoCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactCommentGroupPostVideoCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactCommentGroupPostVideoCommandResult>> Handle(CreateReactCommentGroupPostVideoCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // Lấy thông tin comment
            var comment = await _querycontext.CommentGroupVideoPosts.FirstOrDefaultAsync(c => c.CommentGroupVideoPostId == request.CommentGroupVideoPostId, cancellationToken);
            if (comment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // 1. Kiểm tra phản ứng (reaction) hiện có cho comment
            var existingReact = await _querycontext.ReactGroupVideoPostComments
                .FirstOrDefaultAsync(r =>
                    r.GroupPostVideoId == request.GroupPostVideoId &&
                    r.CommentGroupVideoPostId == request.CommentGroupVideoPostId &&
                    r.UserId == request.UserId,
                    cancellationToken);

            Domain.CommandModels.ReactGroupVideoPostComment ReactComments = new();

            if (existingReact != null)
            {
                // Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Nếu cùng loại reaction, xóa phản ứng
                    var commandReact = new Domain.CommandModels.ReactGroupVideoPostComment
                    {
                        ReactGroupVideoCommentId = existingReact.ReactGroupVideoCommentId,
                        GroupPostVideoId = existingReact.GroupPostVideoId,
                        CommentGroupVideoPostId = existingReact.CommentGroupVideoPostId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate
                    };
                    _context.ReactGroupVideoPostComments.Remove(commandReact);
                }
                else
                {
                    // Nếu khác loại, cập nhật loại reaction và thời gian
                    var commandReact = new Domain.CommandModels.ReactGroupVideoPostComment
                    {
                        ReactGroupVideoCommentId = existingReact.ReactGroupVideoCommentId,
                        GroupPostVideoId = existingReact.GroupPostVideoId,
                        CommentGroupVideoPostId = existingReact.CommentGroupVideoPostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _context.ReactGroupVideoPostComments.Update(commandReact);
                }
            }
            else
            {
                // Tạo phản ứng mới
                ReactComments = new Domain.CommandModels.ReactGroupVideoPostComment
                {
                    ReactGroupVideoCommentId = _helper.GenerateNewGuid(),
                    GroupPostVideoId = request.GroupPostVideoId,
                    CommentGroupVideoPostId = request.CommentGroupVideoPostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactGroupVideoPostComments.AddAsync(ReactComments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactCommentGroupPostVideoCommandResult>(existingReact)
                : _mapper.Map<CreateReactCommentGroupPostVideoCommandResult>(ReactComments);

            return Result<CreateReactCommentGroupPostVideoCommandResult>.Success(result);
        }

    }
}
