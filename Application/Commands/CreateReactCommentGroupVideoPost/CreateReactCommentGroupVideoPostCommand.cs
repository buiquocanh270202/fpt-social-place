using Application.Commands.CreateNewReact;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.CreateReactCommentGroupVideoPost
{
    public class CreateReactCommentGroupPostVideoCommand : ICommand<CreateReactCommentGroupPostVideoCommandResult>
    {
        public Guid GroupPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentGroupVideoPostId { get; set; }
    }
}
