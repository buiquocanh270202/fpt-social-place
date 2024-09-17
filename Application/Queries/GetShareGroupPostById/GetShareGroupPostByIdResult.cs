using Application.DTO.GetUserProfileDTO;
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

namespace Application.Queries.GetShareGroupPostById
{
    public class GetShareGroupPostByIdResult
    {
        public Guid GroupSharePostId { get; set; }
        public string? Content { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? SharedToUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsHide { get; set; }
        public bool? IsBanned { get; set; }
        public bool? IsPending { get; set; }
        public virtual UserPostDTO? UserPostShare { get; set; }
        public virtual UserPostPhotoDTO? UserPostPhotoShare { get; set; }
        public virtual UserPostVideoDTO? UserPostVideoShare { get; set; }
        public virtual GroupPostDTO? GroupPostShare { get; set; }
        public virtual GroupPostPhotoDTO? GroupPostPhotoShare { get; set; }
        public virtual GroupPostVideoDTO? GroupPostVideoShare { get; set; }
        public Guid? UserSharedId { get; set; }
        public virtual string? UserNameShare { get; set; }
        public virtual GetUserAvatar? UserAvatarShare { get; set; }
        public Guid? GroupShareId { get; set; }
        public string? GroupShareName { get; set; }
        public string? GroupShareCorverImage { get; set; }
        public Guid UserId { get; set; }
        public virtual string? UserName { get; set; }
        public virtual GetUserAvatar? UserAvatar { get; set; }
        public GetGroupStatusDTO? GroupStatus { get; set; }
        public Guid? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCorverImage { get; set; }
        public ReactCount? ReactCount { get; set; }
    }
}
