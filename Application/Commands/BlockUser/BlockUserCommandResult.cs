using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.BlockUser
{
    public class BlockUserCommandResult
    {
        public string Message { get; set; }
        public bool IsBlocked { get; set; }
    }
}
