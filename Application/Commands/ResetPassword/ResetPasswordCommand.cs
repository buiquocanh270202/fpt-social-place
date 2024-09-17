using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ResetPassword
{
    public class ResetPasswordCommand : ICommand<ResetPasswordCommandResult>
    {
        public string Username { get; set; }
    }
}
