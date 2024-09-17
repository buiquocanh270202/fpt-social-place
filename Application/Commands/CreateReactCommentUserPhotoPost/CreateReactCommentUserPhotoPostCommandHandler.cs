using Application.Commands.CreateNewReact;
using Application.Commands.CreateReactCommentUserPost;
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

namespace Application.Commands.CreateReactCommentUserPostPhoto
{
    public class CreateReactCommentUserPostPhotoCommandHandler : ICommandHandler<CreateReactCommentUserPostPhotoCommand, CreateReactCommentUserPostPhotoCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReactCommentUserPostPhotoCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReactCommentUserPostPhotoCommandResult>> Handle(CreateReactCommentUserPostPhotoCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var comment = await _querycontext.CommentPhotoPosts.FirstOrDefaultAsync(c => c.CommentPhotoPostId == request.CommentPhotoPostId, cancellationToken);
            if (comment == null)
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // 1. Kiểm tra phản ứng (reaction) hiện có cho comment
            var existingReact = await _querycontext.ReactPhotoPostComments
                .FirstOrDefaultAsync(r =>
                    r.UserPostPhotoId == request.UserPostPhotoId &&
                    r.CommentPhotoPostId == request.CommentPhotoPostId &&
                    r.UserId == request.UserId,
                    cancellationToken);

            Domain.CommandModels.ReactPhotoPostComment ReactComments = new();

            if (existingReact != null)
            {
                // 2. Xử lý phản ứng hiện có
                if (existingReact.ReactTypeId == request.ReactTypeId)
                {
                    // Nếu cùng loại reaction, xóa phản ứng
                    var commandReact = new Domain.CommandModels.ReactPhotoPostComment
                    {
                        ReactPhotoPostCommentId = existingReact.ReactPhotoPostCommentId,
                        UserPostPhotoId = existingReact.UserPostPhotoId,
                        CommentPhotoPostId = existingReact.CommentPhotoPostId,
                        ReactTypeId = existingReact.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = existingReact.CreatedDate
                    };
                    _context.ReactPhotoPostComments.Remove(commandReact);
                }
                else
                {
                    // Nếu khác loại, cập nhật loại reaction và thời gian
                    var commandReact = new Domain.CommandModels.ReactPhotoPostComment
                    {
                        ReactPhotoPostCommentId = existingReact.ReactPhotoPostCommentId,
                        UserPostPhotoId = existingReact.UserPostPhotoId,
                        CommentPhotoPostId = existingReact.CommentPhotoPostId,
                        ReactTypeId = request.ReactTypeId,
                        UserId = existingReact.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _context.ReactPhotoPostComments.Update(commandReact);
                }
            }
            else
            {
                // 3. Tạo phản ứng mới
                ReactComments = new Domain.CommandModels.ReactPhotoPostComment
                {
                    ReactPhotoPostCommentId = _helper.GenerateNewGuid(),
                    UserPostPhotoId = request.UserPostPhotoId,
                    CommentPhotoPostId = request.CommentPhotoPostId,
                    ReactTypeId = request.ReactTypeId,
                    UserId = request.UserId,
                    CreatedDate = DateTime.Now
                };

                await _context.ReactPhotoPostComments.AddAsync(ReactComments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // 5. Trả về kết quả
            var result = existingReact != null
                ? _mapper.Map<CreateReactCommentUserPostPhotoCommandResult>(existingReact)
                : _mapper.Map<CreateReactCommentUserPostPhotoCommandResult>(ReactComments);


            return Result<CreateReactCommentUserPostPhotoCommandResult>.Success(result);
        }

    }
}
