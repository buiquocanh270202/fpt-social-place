using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteCommentGroupPhotoPost
{
    public class DeleteCommentGroupPhotoPostCommand : ICommand<DeleteCommentGroupPhotoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid CommentPhotoGroupPostId { get; set; }
    }
}
