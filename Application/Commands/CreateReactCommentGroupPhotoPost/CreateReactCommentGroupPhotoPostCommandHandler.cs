using Application.Commands.CreateNewReact;
using Application.Commands.CreateReactCommentGroupPost;
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

namespace Application.Commands.CreateReactCommentGroupPostPhoto
{
    public class CreateReactCommentGroupPostPhotoCommandHandler : ICommandHandler<CreateReactCommentGroupPostPhotoCommand, CreateReactCommentGroupPostPhotoCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactCommentGroupPostPhotoCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactCommentGroupPostPhotoCommandResult>> Handle(CreateReactCommentGroupPostPhotoCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            //Lấy thông tin comment
            var comment = await _querycontext.CommentPhotoGroupPosts.FirstOrDefaultAsync(c => c.CommentPhotoGroupPostId == request.CommentPhotoGroupPostId, cancellationToken);
            if (comment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // 1. Kiểm tra phản ứng (reaction) hiện có cho comment
            var existingReact = await _querycontext.ReactGroupPhotoPostComments
                .FirstOrDefaultAsync(r =>
                    r.GroupPostPhotoId == request.GroupPostPhotoId &&
                    r.CommentPhotoGroupPostId == request.CommentPhotoGroupPostId &&
                    r.UserId == request.UserId,
                    cancellationToken);

            Domain.CommandModels.ReactGroupPhotoPostComment ReactComments = new();

            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Nếu cùng loại reaction, xóa phản ứng
                    var commandReact = new Domain.CommandModels.ReactGroupPhotoPostComment
                    {
                        ReactPhotoPostCommentId = existingReact.ReactPhotoPostCommentId,
                        GroupPostPhotoId = existingReact.GroupPostPhotoId,
                        CommentPhotoGroupPostId = existingReact.CommentPhotoGroupPostId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate
                    };
                    _context.ReactGroupPhotoPostComments.Remove(commandReact);
                }
                else
                {
                    // Nếu khác loại, cập nhật loại reaction và thời gian
                    var commandReact = new Domain.CommandModels.ReactGroupPhotoPostComment
                    {
                        ReactPhotoPostCommentId = existingReact.ReactPhotoPostCommentId,
                        GroupPostPhotoId = existingReact.GroupPostPhotoId,
                        CommentPhotoGroupPostId = existingReact.CommentPhotoGroupPostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _context.ReactGroupPhotoPostComments.Update(commandReact);

                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                ReactComments = new Domain.CommandModels.ReactGroupPhotoPostComment
                {
                    ReactPhotoPostCommentId = _helper.GenerateNewGuid(),
                    GroupPostPhotoId = request.GroupPostPhotoId,
                    CommentPhotoGroupPostId = request.CommentPhotoGroupPostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactGroupPhotoPostComments.AddAsync(ReactComments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // 5. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactCommentGroupPostPhotoCommandResult>(existingReact)
                : _mapper.Map<CreateReactCommentGroupPostPhotoCommandResult>(ReactComments);


            return Result<CreateReactCommentGroupPostPhotoCommandResult>.Success(result);
        }

    }
}
