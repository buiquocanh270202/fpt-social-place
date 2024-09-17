using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateRoleMember
{
    public class UpdateRoleMemberCommand : ICommand<UpdateRoleMemberCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid GroupId { get; set; }
        public Guid MemberId { get; set; }
        public Guid? GroupRoleId { get; set; }
        public int Action { get; set; }
    }
}
