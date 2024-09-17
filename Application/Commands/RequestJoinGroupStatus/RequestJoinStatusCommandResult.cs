using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.RequestJoinGroupStatus
{
    public class RequestJoinStatusCommandResult
    {
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public bool IsJoin { get; set; }
    }
}
