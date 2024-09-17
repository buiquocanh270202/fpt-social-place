using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.InvatedFriendToGroup
{
    public class InvatedMemberCommandResult
    {
        public InvatedMemberCommandResult()
        {
            MemberId = new List<Guid>();
        }
        public string Message { get; set; }
        public List<Guid> MemberId { get; set; }
        public bool Invated { get; set; }
    }
}
