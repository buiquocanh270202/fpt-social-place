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

namespace Application.Commands.CreateReactForCommentSharePost
{
    public class CreateReactForCommentSharePostCommandHandler : ICommandHandler<CreateReactForCommentSharePostCommand, CreateReactForCommentSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactForCommentSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactForCommentSharePostCommandResult>> Handle(CreateReactForCommentSharePostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // 4. Lấy thông tin comment
            var comment = await _querycontext.CommentSharePosts.FirstOrDefaultAsync(c => c.CommentSharePostId == request.CommentSharePostId, cancellationToken);
            if (comment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // 1. Kiểm tra phản ứng (reaction) hiện có cho comment
            var existingReact = await _querycontext.ReactSharePostComments
                .FirstOrDefaultAsync(r =>
                    r.SharePostId == request.SharePostId &&
                    r.CommentSharePostId == request.CommentSharePostId &&
                    r.UserId == request.UserId,
                    cancellationToken);

            Domain.CommandModels.ReactSharePostComment ReactComments = new();

            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Nếu cùng loại reaction, xóa phản ứng
                    var commandReact = new Domain.CommandModels.ReactSharePostComment
                    {
                        ReactSharePosCommentId = existingReact.ReactSharePosCommentId,
                        SharePostId = existingReact.SharePostId,
                        CommentSharePostId = existingReact.CommentSharePostId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreateDate = existingReact.CreateDate,
                    };
                    _context.ReactSharePostComments.Remove(commandReact);
                }
                else
                {
                    // Nếu khác loại, cập nhật loại reaction và thời gian
                    var commandReact = new Domain.CommandModels.ReactSharePostComment
                    {
                        ReactSharePosCommentId = existingReact.ReactSharePosCommentId,
                        SharePostId = existingReact.SharePostId,
                        CommentSharePostId = existingReact.CommentSharePostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreateDate = DateTime.Now
                    };
                    _context.ReactSharePostComments.Update(commandReact);
                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                ReactComments = new Domain.CommandModels.ReactSharePostComment
                {
                    ReactSharePosCommentId = _helper.GenerateNewGuid(),
                    SharePostId = request.SharePostId,
                    CommentSharePostId = request.CommentSharePostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreateDate = DateTime.Now
                };

                await _context.ReactSharePostComments.AddAsync(ReactComments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // 5. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactForCommentSharePostCommandResult>(existingReact)
                : _mapper.Map<CreateReactForCommentSharePostCommandResult>(ReactComments);


            return Result<CreateReactForCommentSharePostCommandResult>.Success(result);
        }
    }
}
