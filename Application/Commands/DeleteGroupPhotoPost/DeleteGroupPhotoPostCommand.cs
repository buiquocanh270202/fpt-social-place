using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.DeleteGroupPhotoPost
{
    public class DeleteGroupPhotoPostCommand : ICommand<DeleteGroupPhotoPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
    }
}
