using Application.Commands.CreateInterest;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactUserPost
{
    public class CreateReactUserPostCommand : ICommand<CreateReactUserPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid ReactTypeId {  get; set; }

    }
}
