using Application.Commands.CreateInterest;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactGroupVideoPost
{
    public class CreateReactGroupVideoPostCommand : ICommand<CreateReactGroupVideoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public Guid ReactTypeId {  get; set; }

    }
}
