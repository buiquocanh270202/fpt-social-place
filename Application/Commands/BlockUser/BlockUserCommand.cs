using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.BlockUser
{
    public class BlockUserCommand : ICommand<BlockUserCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid BlockUserId { get; set; }
    }
}
