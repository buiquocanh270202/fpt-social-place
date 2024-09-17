using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CQRS.Command;

namespace Application.Commands.DeleteChat
{
    public class DeleteChatCommand : ICommand<DeleteChatCommandResult>
    {
        public Guid? UserId { get; set; }
        public int ChatId { get; set; }
    }
}
