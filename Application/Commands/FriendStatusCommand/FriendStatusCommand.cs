using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.FriendStatusCommand
{
    public class FriendStatusCommand : ICommand<FriendStatusCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid FriendId { get; set; }
        public bool? Confirm { get; set; }
        public bool? Cancle { get; set; }
        public bool? Reject { get; set; }
    }
}
