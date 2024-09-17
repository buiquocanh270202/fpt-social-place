using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteCommentUserPhotoPost
{
    public class DeleteCommentUserPhotoPostCommand : ICommand<DeleteCommentUserPhotoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid CommentPhotoPostId { get; set; }
    }
}
