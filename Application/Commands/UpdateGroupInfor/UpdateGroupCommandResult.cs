using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateGroupInfor
{
    public class UpdateGroupCommandResult
    {
        public string Message { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
