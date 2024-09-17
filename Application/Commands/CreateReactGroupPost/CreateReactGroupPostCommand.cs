using Application.Commands.CreateInterest;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactGroupPost
{
    public class CreateReactGroupPostCommand : ICommand<CreateReactGroupPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid ReactTypeId {  get; set; }

    }
}
