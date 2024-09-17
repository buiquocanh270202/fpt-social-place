
using Application.DTO.GetUserProfileDTO;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UserPostDTO
{
    public class UserPostDTO
    {
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public string? UserPostNumber { get; set; }
        public Guid UserStatusId { get; set; }
        public bool? IsAvataPost { get; set; }
        public bool? IsCoverPhotoPost { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? PhotoId { get; set; }
        public Guid? VideoId { get; set; }
        public int? NumberPost { get; set; }
        public virtual UserPostPhotoDTO.PhotoDTO? Photo { get; set; }
        public virtual UserPostVideoDTO.VideoDTO? Video { get; set; }
        public virtual ICollection<UserPostPhotoDTO.UserPostPhotoDTO>? UserPostPhotos { get; set; }
        public virtual ICollection<UserPostVideoDTO.UserPostVideoDTO>? UserPostVideos { get; set; }
        public virtual GetUserAvatar? Avatar { get; set; }
        public string? FullName { get; set; }
    }
}
