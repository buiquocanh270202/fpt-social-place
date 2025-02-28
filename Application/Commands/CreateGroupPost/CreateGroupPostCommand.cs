﻿using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Core.CQRS.Command;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.CreateGroupPost
{
    public class CreateGroupPostCommand : ICommand<CreateGroupPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public string Content { get; set; }
        public List<PhotoAddOnPost>? Photos { get; set; }
        public List<VideoAddOnPost>? Videos { get; set; }

    }
}
