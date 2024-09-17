using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateClient
{
    public class CreateClientCommand :ICommand<CreateClientCommandResult>
    {
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public int ClientType { get; set; }
    }
}
