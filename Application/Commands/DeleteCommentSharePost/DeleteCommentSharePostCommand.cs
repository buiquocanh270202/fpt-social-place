using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteCommentSharePost
{
    public class DeleteCommentSharePostCommand : ICommand<DeleteCommentSharePostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid CommentSharePostId { get; set; }
    }
}
