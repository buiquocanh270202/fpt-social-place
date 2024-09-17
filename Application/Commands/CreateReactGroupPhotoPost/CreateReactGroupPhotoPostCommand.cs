using Application.Commands.CreateInterest;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactGroupPhotoPost
{
    public class CreateReactGroupPhotoPostCommand : ICommand<CreateReactGroupPhotoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public Guid ReactTypeId {  get; set; }

    }
}
