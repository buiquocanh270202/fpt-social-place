using Application.Commands.CreateNewReact;
using Application.Commands.CreateReactCommentUserPost;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.CreateReactCommentUserPostPhoto
{
    public class CreateReactCommentUserPostPhotoCommand : ICommand<CreateReactCommentUserPostPhotoCommandResult>
    {
        public Guid UserPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentPhotoPostId { get; set; }
    }
}
