using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportProfile
{
    public class CreateReportProfileCommandResult
    {
        public Guid ReportProfileId { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? UserId { get; set; }
        public Guid ReportById { get; set; }
        public bool? ReportStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Processing { get; set; }
    }
}
