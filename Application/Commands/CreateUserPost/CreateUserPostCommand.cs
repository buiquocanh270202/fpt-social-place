using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Core.CQRS.Command;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.Post
{
    public class CreateUserPostCommand : ICommand<CreateUserPostCommandResult>
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Guid UserStatusId { get; set; }
        public bool IsAvataPost { get; set; }
        public bool IsCoverPhotoPost { get; set; }
        public List<PhotoAddOnPost>? Photos { get; set; }
        public List<VideoAddOnPost>? Videos { get; set; }

    }
}
