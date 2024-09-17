using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateAccount
{
    public class CreateAccountCommand : ICommand<CreateAccountCommandResult>
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RollNumber { get; set; }
        public string Campus { get; set; }
    }
}
