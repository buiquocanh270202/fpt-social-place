using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportType
{
    public class CreateReportTypeCommandResult
    {
        public Guid ReportTypeId { get; set; }
        public string ReportTypeName { get; set; } = string.Empty;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
