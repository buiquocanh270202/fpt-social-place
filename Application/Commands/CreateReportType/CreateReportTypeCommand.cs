using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportType
{
    public class CreateReportTypeCommand : ICommand<CreateReportTypeCommandResult>
    {
        public string ReportTypeName { get; set; }
    }
}
