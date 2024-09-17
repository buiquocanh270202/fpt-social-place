using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportComment
{
    public class CreateReportCommentCommandHandler : ICommandHandler<CreateReportCommentCommand, CreateReportCommentCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReportCommentCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReportCommentCommandResult>> Handle(CreateReportCommentCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null) {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reportCmt = new Domain.CommandModels.ReportComment();
            reportCmt.ReportCommentId = _helper.GenerateNewGuid();
            reportCmt.ReportTypeId = request.ReportTypeId;
            reportCmt.CommentId =  request.CommentId;
            reportCmt.CommentPhotoPostId =  request.CommentPhotoPostId;
            reportCmt.CommentVideoPostId = request.CommentVideoPostId;
            reportCmt.CommentGroupPostId = request.CommentGroupPostId;
            reportCmt.CommentPhotoGroupPostId =  request.CommentPhotoGroupPostId;
            reportCmt.CommentGroupVideoPostId = request.CommentGroupVideoPostId;
            reportCmt.CommentSharePostId = request.CommentSharePostId;
            reportCmt.CommentGroupSharePostId =request.CommentGroupSharePostId;
            reportCmt.Content = request.Content;

            reportCmt.Content = request.CommentId != null ? _querycontext.CommentPosts.Where(x => x.CommentId == request.CommentId).Select(x => x.Content).FirstOrDefault() :
                        request.CommentPhotoPostId != null ? _querycontext.CommentPhotoPosts.Where(x => x.CommentPhotoPostId == request.CommentPhotoPostId).Select(x => x.Content).FirstOrDefault() :
                        request.CommentVideoPostId != null ? _querycontext.CommentVideoPosts.Where(x => x.CommentVideoPostId == request.CommentVideoPostId).Select(x => x.Content).FirstOrDefault() :
                        request.CommentGroupPostId != null? _querycontext.CommentGroupPosts.Where(x => x.CommentGroupPostId == request.CommentGroupPostId).Select(x => x.Content).FirstOrDefault() :
                        request.CommentPhotoGroupPostId != null ? _querycontext.CommentPhotoGroupPosts.Where(x => x.CommentPhotoGroupPostId == request.CommentPhotoGroupPostId).Select(x => x.Content).FirstOrDefault() :
                        request.CommentGroupVideoPostId != null ? _querycontext.CommentGroupVideoPosts.Where(x => x.CommentGroupVideoPostId == request.CommentGroupVideoPostId).Select(x => x.Content).FirstOrDefault() :
                        request.CommentSharePostId != null ? _querycontext.CommentSharePosts.Where(x => x.CommentSharePostId == request.CommentSharePostId).Select(x => x.Content).FirstOrDefault() :
                        request.CommentGroupSharePostId != null ? _querycontext.CommentGroupSharePosts.Where(x => x.CommentGroupSharePostId == request.CommentGroupSharePostId).Select(x => x.Content).FirstOrDefault() :
                        string.Empty;

            reportCmt.ReportById = (Guid) request.ReportById;
            reportCmt.ReportStatus = null;
            reportCmt.CreatedDate = DateTime.Now;
            reportCmt.Processing = true;

            await _context.ReportComments.AddAsync(reportCmt);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<CreateReportCommentCommandResult>(reportCmt);

            return Result<CreateReportCommentCommandResult>.Success(result);
        }
    }
}
