using Application.Commands.CreateNewReact;
using Application.Commands.CreateReactCommentGroupPost;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.CreateReactCommentGroupPostPhoto
{
    public class CreateReactCommentGroupPostPhotoCommand : ICommand<CreateReactCommentGroupPostPhotoCommandResult>
    {
        public Guid GroupPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentPhotoGroupPostId { get; set; }
    }
}
