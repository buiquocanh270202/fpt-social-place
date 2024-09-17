using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteCommentGroupPost
{
    public class DeleteCommentGroupPostCommand : ICommand<DeleteCommentGroupPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid CommentGroupPostId { get; set; }
    }
}
