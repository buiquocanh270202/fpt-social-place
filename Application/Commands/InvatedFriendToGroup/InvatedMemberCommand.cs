using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.InvatedFriendToGroup
{
    public class InvatedMemberCommand : ICommand<InvatedMemberCommandResult>
    {
        public InvatedMemberCommand()
        {
            MemberId = new List<Guid>();
        }
        public Guid? UserId {get;set;}
        public  List<Guid> MemberId { get; set; }
        public Guid GroupId { get; set; }
    }
}
