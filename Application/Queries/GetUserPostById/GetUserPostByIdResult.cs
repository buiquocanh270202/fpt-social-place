using Application.DTO.GetUserProfileDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserPostById
{
    public class GetUserPostByIdResult 
    {
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public string? UserPostNumber { get; set; }
        public GetUserStatusDTO? UserStatus { get; set; }
        public bool? IsAvataPost { get; set; }
        public bool? IsCoverPhotoPost { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? PhotoId { get; set; }
        public Guid? VideoId { get; set; }
        public int? NumberPost { get; set; }
        public virtual PhotoDTO? Photo { get; set; }
        public virtual VideoDTO? Video { get; set; }
        public virtual ICollection<UserPostPhotoDTO>? UserPostPhotos { get; set; }
        public virtual ICollection<UserPostVideoDTO>? UserPostVideos { get; set; }
        public virtual GetUserAvatar? Avatar { get; set; }
        public string? FullName { get; set; }
        public ReactCount? ReactCount { get; set; }
    }
}
