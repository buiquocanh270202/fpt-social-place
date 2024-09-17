using Application.Commands.CreateUserCommentPost;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateUserCommentGroupPhotoPost
{
    public class CreateUserCommentGroupPhotoPostCommand : ICommand<CreateUserCommentGroupPhotoPostCommandResult>
    {
        public Guid GroupPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
