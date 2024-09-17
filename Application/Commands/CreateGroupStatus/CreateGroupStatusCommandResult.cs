using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupStatus
{
    public class CreateGroupStatusCommandResult
    {
        public Guid GroupStatusId { get; set; }
        public string GroupStatusName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
}
