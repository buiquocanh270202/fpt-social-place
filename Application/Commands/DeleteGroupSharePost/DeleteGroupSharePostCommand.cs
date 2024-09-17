using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteGroupSharePost
{
    public class DeleteGroupSharePostCommand : ICommand<DeleteGroupSharePostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupSharePostId { get; set; }
    }
}
