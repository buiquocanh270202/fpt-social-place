using Application.Commands.CreateReportComment;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportPost
{
    public class CreateReportPostCommandHandler : ICommandHandler<CreateReportPostCommand, CreateReportPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReportPostCommandHandler(fptforumCommandContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateReportPostCommandResult>> Handle(CreateReportPostCommand request, CancellationToken cancellationToken)
        {
            if(_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null) {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reportPost = new ReportPost();
            reportPost.ReportPostId = _helper.GenerateNewGuid();
            reportPost.ReportTypeId = request.ReportTypeId;
            reportPost.ReportById = (Guid) request.ReportById;
            reportPost.UserPostId = request.UserPostId; 
            reportPost.UserPostPhotoId = request.UserPostPhotoId;
            reportPost.UserPostVideoId = request.UserPostVideoId;
            reportPost.GroupPostId = request.GroupPostId;
            reportPost.GroupPostPhotoId = request.GroupPostPhotoId;
            reportPost.UserPostVideoId = request.UserPostVideoId;
            reportPost.SharePostId = request.SharePostId;
            reportPost.GroupSharePostId = request.GroupSharePostId;
            reportPost.ReportStatus = null;
            reportPost.CreatedDate = DateTime.Now;
            reportPost.Processing = true;

            await _context.ReportPosts.AddAsync(reportPost);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<CreateReportPostCommandResult>(reportPost);

            return Result<CreateReportPostCommandResult>.Success(result);
        }
    }
}
