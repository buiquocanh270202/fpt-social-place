using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportPost
{
    public class CreateReportPostCommand : ICommand<CreateReportPostCommandResult>
    {
        public Guid ReportTypeId { get; set; }
        public Guid? ReportById { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? SharePostId { get; set; }
        public Guid? GroupSharePostId { get; set; }
    }
}
