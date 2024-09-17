using Application.Commands.CreateUserCommentPost;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateUserCommentGroupVideoPost
{
    public class CreateUserCommentGroupVideoPostCommand : ICommand<CreateUserCommentGroupVideoPostCommandResult>
    {
        public Guid GroupPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
