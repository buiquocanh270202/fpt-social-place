using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteGroup
{
    public class DeleteGroupCommand : ICommand<DeleteGroupCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
