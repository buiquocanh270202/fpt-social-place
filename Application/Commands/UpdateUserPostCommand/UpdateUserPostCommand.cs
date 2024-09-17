using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.UpdateUserPostCommand
{
    public class UpdateUserPostCommand : ICommand<UpdateUserPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid UserPostId { get; set; }
        public string Content { get; set; }
        public Guid UserStatusId { get; set; }
        public List<PhotoAddOnPost>? Photos { get; set; }
        public List<VideoAddOnPost>? Videos { get; set; }
    }
}
