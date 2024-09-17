using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteCommentGroupVideoPost
{
    public class DeleteCommentGroupVideoPostCommand : ICommand<DeleteCommentGroupVideoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid CommentGroupVideoPostId { get; set; }
    }
}
