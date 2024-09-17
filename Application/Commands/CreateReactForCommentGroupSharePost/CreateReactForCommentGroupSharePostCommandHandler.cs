using Application.Commands.CreateUserCommentPost;
using Application.Services;
using AutoMapper;
using Core.CQRS.Command;
using Core.CQRS;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Commands.CreateReactCommentUserPost;
using Microsoft.EntityFrameworkCore.Query;
using Domain.QueryModels;

namespace Application.Commands.CreateReactForCommentGroupSharePost
{
    public class CreateReactForCommentGroupSharePostCommandHandler : ICommandHandler<CreateReactForCommentGroupSharePostCommand, CreateReactForCommentGroupSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactForCommentGroupSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactForCommentGroupSharePostCommandResult>> Handle(CreateReactForCommentGroupSharePostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var comment = await _querycontext.CommentGroupSharePosts.FirstOrDefaultAsync(c => c.CommentGroupSharePostId == request.CommentGroupSharePostId, cancellationToken);
            if (comment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // 1. Kiểm tra phản ứng (reaction) hiện có cho comment
            var existingReact = await _querycontext.ReactGroupSharePostComments
                .FirstOrDefaultAsync(r =>
                    r.GroupSharePostId == request.GroupSharePostId &&
                    r.CommentGroupSharePostId == request.CommentGroupSharePostId &&
                    r.UserId == request.UserId,
                    cancellationToken);

            Domain.CommandModels.ReactGroupSharePostComment ReactComments = new();

            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Nếu cùng loại reaction, xóa phản ứng
                    var commandReact = new Domain.CommandModels.ReactGroupSharePostComment
                    {
                        ReactGroupSharePosCommentId = existingReact.ReactGroupSharePosCommentId,
                        GroupSharePostId = existingReact.GroupSharePostId,
                        CommentGroupSharePostId = existingReact.CommentGroupSharePostId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreateDate = existingReact.CreateDate,
                    };
                    _context.ReactGroupSharePostComments.Remove(commandReact);
                }
                else
                {
                    // Nếu khác loại, cập nhật loại reaction và thời gian
                    var commandReact = new Domain.CommandModels.ReactGroupSharePostComment
                    {
                        ReactGroupSharePosCommentId = existingReact.ReactGroupSharePosCommentId,
                        GroupSharePostId = existingReact.GroupSharePostId,
                        CommentGroupSharePostId = existingReact.CommentGroupSharePostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreateDate = DateTime.Now
                    };
                    _context.ReactGroupSharePostComments.Update(commandReact);
                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                ReactComments = new Domain.CommandModels.ReactGroupSharePostComment
                {
                    ReactGroupSharePosCommentId = _helper.GenerateNewGuid(),
                    GroupSharePostId = request.GroupSharePostId,
                    CommentGroupSharePostId = request.CommentGroupSharePostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreateDate = DateTime.Now
                };

                await _context.ReactGroupSharePostComments.AddAsync(ReactComments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // 5. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactForCommentGroupSharePostCommandResult>(existingReact)
                : _mapper.Map<CreateReactForCommentGroupSharePostCommandResult>(ReactComments);


            return Result<CreateReactForCommentGroupSharePostCommandResult>.Success(result);
        }
    }
}
