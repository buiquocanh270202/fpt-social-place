using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupTag
{
    public class CreateGroupTagCommandResult
    {
        public string TagName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
