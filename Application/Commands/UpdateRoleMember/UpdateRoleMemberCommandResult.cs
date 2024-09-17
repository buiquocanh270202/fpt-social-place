using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateRoleMember
{
    public class UpdateRoleMemberCommandResult
    {
        public string Message { get; set; }
        public bool UpdateRole { get; set; }
        public bool IsDelete { get; set; }
    }
}
