using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupRole
{
    public class CreateGroupRoleCommand : ICommand<CreateGroupRoleCommandResult>
    {
        public string GroupRoleName { get; set; }
    }
}
