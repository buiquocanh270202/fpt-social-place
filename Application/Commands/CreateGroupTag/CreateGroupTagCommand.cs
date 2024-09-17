using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupTag
{
    public class CreateGroupTagCommand : ICommand<CreateGroupTagCommandResult>
    {
        public string TagName { get; set; }
    }
}
