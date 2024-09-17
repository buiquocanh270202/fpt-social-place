using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteGroupVideoPost
{
    public class DeleteGroupVideoPostCommand : ICommand<DeleteGroupVideoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupPostVideoId { get; set; }
    }
}
