using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.JoinGroupCommand
{
    public class RequestJoinGroupCommandResult
    {
        public string? Message { get; set; }
        public bool IsRequest { get; set; }
        public bool IsJoin { get; set; }
    }
}
