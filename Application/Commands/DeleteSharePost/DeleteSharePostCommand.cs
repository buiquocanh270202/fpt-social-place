using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteSharePost
{
    public class DeleteSharePostCommand : ICommand<DeleteSharePostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid SharePostId { get; set; }
    }
}
