using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateChat
{
    public class UpdateChatCommandResult
    {
        public int ChatId { get; set; }
        public string ChatName { get; set; }
        public string Message { get; set; }
    }
}
