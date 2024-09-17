using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeactiveUserCommand
{
    public class DeactiveUserCommand : ICommand<DeactiveUserCommandResult>
    {
        public Guid UserId { get; set; }
    }
}
