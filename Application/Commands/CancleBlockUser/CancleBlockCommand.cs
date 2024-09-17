using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CancleBlockUser
{
    public class CancleBlockCommand : ICommand<CancleBlockCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid? BlockedUserId { get; set; }
    }
}
