using Application.Commands.CreateInterest;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactUserVideoPost
{
    public class CreateReactUserVideoPostCommand : ICommand<CreateReactUserVideoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public Guid ReactTypeId {  get; set; }

    }
}
