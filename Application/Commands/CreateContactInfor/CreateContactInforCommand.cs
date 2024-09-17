using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.CreateContactInfor
{
    public class CreateContactInforCommand : ICommand<CreateContactInforCommandResult>
    {
        public string? SecondEmail { get; set; }
        public string PrimaryNumber { get; set; } = null!;
        public string? SecondNumber { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatus { get; set; }
    }
}
