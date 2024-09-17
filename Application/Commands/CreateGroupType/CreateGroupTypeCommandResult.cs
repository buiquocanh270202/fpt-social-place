using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupType
{
    public class CreateGroupTypeCommandResult
    {
        public Guid GroupTypeId { get; set; }
        public string GroupTypeName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
