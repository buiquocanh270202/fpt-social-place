using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateUserGender
{
    public class CreateUserGenderCommand : ICommand<CreateUserGenderCommandResult>
    {
        public Guid GenderId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
    }
}
