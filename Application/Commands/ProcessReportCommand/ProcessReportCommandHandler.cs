using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ProcessReportCommand
{
    public class ProcessReportCommandHandler : ICommandHandler<ProcessReportCommand, ProcessReportCommandResult>
    {
        private readonly fptforumCommandContext _context;

        public ProcessReportCommandHandler(fptforumCommandContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<Result<ProcessReportCommandResult>> Handle(ProcessReportCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            switch (request.ReportType)
            {
                case "Comment":
                    await ProcessCommentReport(request);
                    break;
                case "Post":
                    await ProcessPostReport(request);
                    break;
                case "User":
                    await ProcessUserReport(request);
                    break;
                case "Group":
                    await ProcessGroupReport(request);
                    break;
                default:
                    throw new ErrorException(StatusCodeEnum.PR01_Invalid_Report_Type);
            }

            await _context.SaveChangesAsync();

            return Result<ProcessReportCommandResult>.Success(new ProcessReportCommandResult
            {
                Success = true
            });
        }

        private async Task ProcessCommentReport(ProcessReportCommand request)
        {
            var report = new List<Domain.CommandModels.ReportComment>();
            if (request.CommentId != null)
            {
                report = await _context.ReportComments.Where(r => r.CommentId == request.CommentId).ToListAsync();
            }
            else if (request.CommentPhotoPostId != null)
            {
                report = await _context.ReportComments.Where(r => r.CommentPhotoPostId == request.CommentPhotoPostId).ToListAsync();
            }
            else if (request.CommentVideoPostId != null)
            {
                report = await _context.ReportComments.Where(r => r.CommentVideoPostId == request.CommentVideoPostId).ToListAsync();
            }
            else if (request.CommentGroupPostId != null)
            {
                report = await _context.ReportComments.Where(r => r.CommentGroupPostId == request.CommentGroupPostId).ToListAsync();
            }
            else if (request.CommentPhotoGroupPostId != null)
            {
                report = await _context.ReportComments.Where(r => r.CommentPhotoGroupPostId == request.CommentPhotoGroupPostId).ToListAsync();
            }
            else if (request.CommentGroupVideoPostId != null)
            {
                report = await _context.ReportComments.Where(r => r.CommentGroupVideoPostId == request.CommentGroupVideoPostId).ToListAsync();
            }
            else if (request.CommentSharePostId != null)
            {
                report = await _context.ReportComments.Where(r => r.CommentSharePostId == request.CommentSharePostId).ToListAsync();
            }
            else if (request.CommentGroupSharePostId != null)
            {
                report = await _context.ReportComments.Where(r => r.CommentGroupSharePostId == request.CommentGroupSharePostId).ToListAsync();
            }
            else
            {
                throw new ErrorException("CommentId must be required!");
            }

            if (report == null)
            {
                throw new ErrorException(StatusCodeEnum.RP01_Report_Not_Found);
            }

            foreach (var item in report)
            {
                item.ReportStatus = false;
                item.Processing = false;

                if (request.IsAccepted == true)
                {
                    item.ReportStatus = true;
                }
            }
        }

        private async Task ProcessPostReport(ProcessReportCommand request)
        {
            var report = new List<Domain.CommandModels.ReportPost>();
            if(request.UserPostId != null)
            {
                report = await _context.ReportPosts.Where(r => r.UserPostId == request.UserPostId).ToListAsync();
            }
            else if (request.UserPostPhotoId != null)
            {
                report = await _context.ReportPosts.Where(r => r.UserPostPhotoId == request.UserPostPhotoId).ToListAsync();
            }
            else if (request.UserPostVideoId != null)
            {
                report = await _context.ReportPosts.Where(r => r.UserPostVideoId == request.UserPostVideoId).ToListAsync();
            }
            else if (request.GroupPostId != null)
            {
                report = await _context.ReportPosts.Where(r => r.GroupPostId == request.GroupPostId).ToListAsync();
            }
            else if (request.GroupPostPhotoId != null)
            {
                report = await _context.ReportPosts.Where(r => r.GroupPostPhotoId == request.GroupPostPhotoId).ToListAsync();
            }
            else if (request.GroupPostVideoId != null)
            {
                report = await _context.ReportPosts.Where(r => r.UserPostId == request.UserPostId).ToListAsync();
            }
            else if (request.SharePostId != null)
            {
                report = await _context.ReportPosts.Where(r => r.SharePostId == request.SharePostId).ToListAsync();
            }
            else if (request.GroupSharePostId != null)
            {
                report = await _context.ReportPosts.Where(r => r.GroupSharePostId == request.GroupSharePostId).ToListAsync();
            }
            else
            {
                throw new ErrorException("PostId must be required!");
            }

            if (report == null)
            {
                throw new ErrorException(StatusCodeEnum.RP01_Report_Not_Found);
            }

            foreach(var item in report)
            {
                item.ReportStatus = false;
                item.Processing = false;

                if (request.IsAccepted == true)
                {
                    item.ReportStatus = true;
                }
            }
        }

        private async Task ProcessUserReport(ProcessReportCommand request)
        {
            if (request.UserId == null)
            {
                throw new ErrorException("UserId must be required!");
            }
            var report = await _context.ReportProfiles.Where(r => r.UserId == request.UserId).ToListAsync();
            if (report == null)
            {
                throw new ErrorException(StatusCodeEnum.RP01_Report_Not_Found);
            }
            foreach(var item in report)
            {
                item.ReportStatus = false;
                item.Processing = false;

                if (request.IsAccepted == true)
                {
                    item.ReportStatus = true;
                }
            }

            // Deactivate user
            /*var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == report.UserId);
            if (user != null)
            {
                user.IsActive = false;
            }*/
        }

        private async Task ProcessGroupReport(ProcessReportCommand request)
        {
            if (request.GroupId == null)
            {
                throw new ErrorException("GroupId must be required!");
            }

            var report = await _context.ReportProfiles.Where(r => r.GroupId == request.GroupId).ToListAsync();
            if (report == null)
            {
                throw new ErrorException(StatusCodeEnum.RP01_Report_Not_Found);
            }
            foreach (var item in report)
            {
                item.ReportStatus = false;
                item.Processing = false;

                if (request.IsAccepted == true)
                {
                    item.ReportStatus = true;
                }
            }
        }
    }
}
