﻿using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupPostIdPendingByGroupId
{
    public class GetGroupPostIdPendingByGroupIdResult
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? IsHide { get; set; }
        public bool? IsBanned { get; set; }
        public bool? IsShare { get; set; }
        public bool? IsPending { get; set; }


        public string? GroupPostNumber { get; set; }
        public int? NumberGroupPost { get; set; }
        public virtual GroupPhotoDTO? GroupPhoto { get; set; }
        public virtual GroupVideoDTO? GroupVideo { get; set; }
        public virtual ICollection<GroupPostPhotoDTO>? GroupPostPhoto { get; set; }
        public virtual ICollection<GroupPostVideoDTO>? GroupPostVideo { get; set; }

        // Specific to SharePost
        public Guid? UserPostShareId { get; set; }
        public Guid? UserPostVideoShareId { get; set; }
        public Guid? UserPostPhotoShareId { get; set; }
        public Guid? GroupPostShareId { get; set; }
        public Guid? GroupPostPhotoShareId { get; set; }
        public Guid? GroupPostVideoShareId { get; set; }
        public Guid? SharedToUserId { get; set; }
        public virtual GroupPhotoDTO? GroupPhotoShare { get; set; }
        public virtual GroupVideoDTO? GroupVideoShare { get; set; }

        //public virtual UserProfile? SharedToUser { get; set; }
        public virtual UserPostDTO? UserPostShare { get; set; }
        public virtual UserPostPhotoDTO? UserPostPhotoShare { get; set; }
        public virtual UserPostVideoDTO? UserPostVideoShare { get; set; }
        public virtual GroupPostDTO? GroupPostShare { get; set; }
        public virtual GroupPostPhotoDTO? GroupPostPhotoShare { get; set; }
        public virtual GroupPostVideoDTO? GroupPostVideoShare { get; set; }
        public virtual string? UserNameShare { get; set; }
        public virtual GetUserAvatar? UserAvatarShare { get; set; }
        public Guid? GroupShareId { get; set; }
        public string? GroupShareName { get; set; }
        public string? GroupShareCorverImage { get; set; }

        //General
        public virtual string? UserName { get; set; }
        public virtual GetUserAvatar? UserAvatar { get; set; }
        public GetGroupStatusDTO? GroupStatus { get; set; }
        public Guid? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCorverImage { get; set; }
    }
}
