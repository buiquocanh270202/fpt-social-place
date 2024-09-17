using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.UpdateUserPhotoPost
{
    public class UpdateUserPhotoPostCommand : ICommand<UpdateUserPhotoPostCommandResult>
    {
        public Guid UserPostPhotoId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
    }
}
