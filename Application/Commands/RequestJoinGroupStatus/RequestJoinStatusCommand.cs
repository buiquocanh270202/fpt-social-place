using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.RequestJoinGroupStatus
{
    public class RequestJoinStatusCommand : ICommand<RequestJoinStatusCommandResult>
    {
        public Guid? ManagerId { get; set; }
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public bool IsJoin { get; set; }
    }
}
