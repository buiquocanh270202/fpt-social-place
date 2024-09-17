using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteCommentGroupSharePost
{
    public class DeleteCommentGroupSharePostCommand : ICommand<DeleteCommentGroupSharePostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid CommentGroupSharePostId { get; set; }
    }
}
