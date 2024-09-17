using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ProcessReportCommand
{
    public class ProcessReportCommand : ICommand<ProcessReportCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? SharePostId { get; set; }
        public Guid? GroupSharePostId { get; set; }
        public Guid? CommentId { get; set; }
        public Guid? CommentPhotoPostId { get; set; }
        public Guid? CommentVideoPostId { get; set; }
        public Guid? CommentGroupPostId { get; set; }
        public Guid? CommentPhotoGroupPostId { get; set; }
        public Guid? CommentGroupVideoPostId { get; set; }
        public Guid? CommentSharePostId { get; set; }
        public Guid? CommentGroupSharePostId { get; set; }
        public string? ReportType { get; set; }
        public bool IsAccepted { get; set; }
    }
}
