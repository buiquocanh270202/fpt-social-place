using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddFriendCommand
{
    public class AddFriendCommandResult
    {
        public Guid UserId { get; set; }
        public string SendBy { get; set; }
        public Guid FriendId { get; set; }
        public string ReceiptBy { get; set; }
        public bool Confirm { get; set; }
    }
}
