using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.UpdateCommentUserVideoPost
{
    public class UpdateCommentUserVideoPostCommand : ICommand<UpdateCommentUserVideoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public string? Content {  get; set; }
        public Guid CommentVideoPostId {  get; set; } 
    }
}
