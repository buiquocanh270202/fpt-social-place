using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteGroupPost
{
    public class DeleteGroupPostCommand : ICommand<DeleteGroupPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupPostId { get; set; }
    }
}
