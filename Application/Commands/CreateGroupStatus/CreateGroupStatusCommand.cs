using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupStatus
{
    public class CreateGroupStatusCommand : ICommand<CreateGroupStatusCommandResult>
    {
        public string GroupStatusName { get; set; }
    }
}
