using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CancleRequestJoinToGroup
{
    public class CancleRequestJoinCommand : ICommand<CancleRequestJoinCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
