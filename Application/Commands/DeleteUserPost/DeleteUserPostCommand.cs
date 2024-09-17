using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteUserPost
{
    public class DeleteUserPostCommand : ICommand<DeleteUserPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid UserPostId { get; set; }
    }
}
