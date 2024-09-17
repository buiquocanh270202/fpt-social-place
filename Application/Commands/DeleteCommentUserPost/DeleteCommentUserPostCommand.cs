using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteCommentUserPost
{
    public class DeleteCommentUserPostCommand : ICommand<DeleteCommentUserPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
    }
}
