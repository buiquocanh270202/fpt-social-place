using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateChatBox
{
    public class CreateChatBoxCommand : ICommand<CreateChatBoxCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid OtherId { get; set; }
        public string Title { get; set; }
    }
}
