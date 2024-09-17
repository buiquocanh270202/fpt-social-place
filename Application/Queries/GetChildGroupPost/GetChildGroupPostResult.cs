using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetChildGroupPost
{
    public class GetChildGroupPostResult
    {
        public Guid GroupPostMediaId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid GroupMediaId { get; set; }
        public string? Content { get; set; }
        public string? GroupPostMediaNumber { get; set; }
        public GetGroupStatusDTO? Status { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public string? GroupMediaType { get; set; }

        public GroupPhotoDTO? GroupPhoto { get; set; }
        public GroupVideoDTO? GroupVideo { get; set; }

        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public GetUserAvatar? Avatar { get; set; }
        public Guid? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCoverImge { get; set; }
        public ReactCount? ReactCount { get; set; }

        public Guid? PreviousId { get; set; }
        public string? PreviousType { get; set; }
        public Guid? NextId { get; set; }
        public string? NextType { get; set; }
    }
}
