﻿using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateGroupSharePost
{
    public class UpdateGroupSharePostCommandResult
    {
        public Guid GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? GroupSharedToUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UserStatusId { get; set; }
        public List<CheckingBadWord.BannedWord>? BannedWords { get; set; }
        public Guid? UserGroupSharedId { get; set; }
    }
}
