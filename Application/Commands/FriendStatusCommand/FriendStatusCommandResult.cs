using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.FriendStatusCommand
{
    public class FriendStatusCommandResult
    {
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public bool Confirm { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
