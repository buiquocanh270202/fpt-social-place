using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.UpdateGroupPhotoPostCommand
{
    public class UpdateGroupPhotoPostCommand : ICommand<UpdateGroupPhotoPostCommandResult>
    {
        public Guid GroupPostPhotoId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
    }
}
