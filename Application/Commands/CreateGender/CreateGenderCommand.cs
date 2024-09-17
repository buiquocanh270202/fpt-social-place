using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGender
{
    public class CreateGenderCommand : ICommand<CreateGenderCommandResult>
    {
        public string GenderName { get; set; }
    }
}
