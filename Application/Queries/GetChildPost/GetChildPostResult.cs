using Application.DTO.GetUserProfileDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetChildPost
{
    public class GetChildPostResult
    {
        public Guid UserPostMediaId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid MediaId { get; set; }
        public string? Content { get; set; }
        public string? UserPostMediaNumber { get; set; }
        public GetUserStatusDTO? Status { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public string MediaType { get; set; }
        public PhotoDTO? Photo { get; set; }
        public VideoDTO? Video { get; set; }

        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public GetUserAvatar Avatar { get; set; }
        public ReactCount ReactCount { get; set; }
        public Guid? PreviousId { get; set; }
        public string? PreviousType { get; set; }
        public Guid? NextId { get; set; }
        public string? NextType { get; set; }
    }
}
