using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateRole
{
    public class CreateRoleCommand : ICommand<CreateRoleCommandResult>
    {
        public string NameRole { get; set; }
    }
}
