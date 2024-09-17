using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.InvatedJoinStatus
{
    public class InvatedJoinStatusCommand : ICommand<InvatedJoinStatusCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid GroupId { get; set; }
        public Guid? InvatedBy { get; set; }
        public bool IsAccept { get; set; }
    }
}
