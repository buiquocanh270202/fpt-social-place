using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetPost
{
    public class GetPostResult
    {
        public List<GetPostDTO>? result { get; set; }
        public int? totalPage { get; set; }
    }

    public class GetPostDTO
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? IsHide { get; set; }
        public bool? IsBanned { get; set; }
        public bool? IsShare { get; set; }
        public bool? IsGroupPost { get; set; }

        // Specific to UserPost
        public string? UserPostNumber { get; set; }
        public Guid? UserStatusId { get; set; }
        public bool? IsAvataPost { get; set; }
        public bool? IsCoverPhotoPost { get; set; }
        public Guid? PhotoId { get; set; }
        public Guid? VideoId { get; set; }
        public int? NumberPost { get; set; }
        public virtual PhotoDTO? Photo { get; set; }
        public virtual VideoDTO? Video { get; set; }
        public virtual List<UserPostPhotoDTO>? UserPostPhoto { get; set; }
        public virtual List<UserPostVideoDTO>? UserPostVideo { get; set; }

        //Specific for GroupPost
        public string? GroupPostNumber { get; set; }
        public GetGroupStatusDTO? GroupStatus { get; set; }
        public int? NumberGroupPost { get; set; }
        public virtual GroupPhotoDTO? GroupPhoto { get; set; }
        public virtual GroupVideoDTO? GroupVideo { get; set; }
        public virtual ICollection<GroupPostPhotoDTO>? GroupPostPhoto { get; set; }
        public virtual ICollection<GroupPostVideoDTO>? GroupPostVideo { get; set; }
        public Guid? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCorverImage { get; set; }

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
        public virtual GetUserStatusDTO? UserStatus { get; set; }
        public ReactCount? ReactCount { get; set; }
        public double? EdgeRank { get; set; }

    }

}