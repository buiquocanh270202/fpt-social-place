using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupRole
{
    public class CreateGroupRoleCommandResult
    {
        public string GroupRoleName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
