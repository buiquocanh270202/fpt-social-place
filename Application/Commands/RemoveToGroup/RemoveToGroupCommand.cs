using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.RemoveToGroup
{
    public class RemoveToGroupCommand : ICommand<RemoveToGroupCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
