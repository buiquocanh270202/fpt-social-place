using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CQRS.Command;

namespace Application.Commands.UpdateChat
{
    public class UpdateChatCommand : ICommand<UpdateChatCommandResult>
    {
        public Guid? UserId { get; set; }
        public string ChatName { get; set; }
        public int ChatId { get; set; } 
    }
}
