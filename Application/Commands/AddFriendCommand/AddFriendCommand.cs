using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddFriendCommand
{
    public class AddFriendCommand : ICommand<AddFriendCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid? FriendId { get; set; }
    }
}
