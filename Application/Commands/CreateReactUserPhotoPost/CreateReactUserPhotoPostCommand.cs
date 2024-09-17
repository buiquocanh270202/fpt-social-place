using Application.Commands.CreateInterest;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactUserPhotoPost
{
    public class CreateReactUserPhotoPostCommand : ICommand<CreateReactUserPhotoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid UserPostPhotoId { get; set; }
        public Guid ReactTypeId {  get; set; }

    }
}
