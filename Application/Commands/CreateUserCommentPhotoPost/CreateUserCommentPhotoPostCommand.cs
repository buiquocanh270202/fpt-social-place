using Application.Commands.CreateUserCommentPost;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateUserCommentPhotoPost
{
    public class CreateUserCommentPhotoPostCommand : ICommand<CreateUserCommentPhotoPostCommandResult>
    {
        public Guid UserPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
