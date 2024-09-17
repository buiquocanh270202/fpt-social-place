using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportProfile
{
    public class CreateReportProfileCommand : ICommand<CreateReportProfileCommandResult>
    {
        public Guid ReportTypeId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ReportById { get; set; }
    }
}
