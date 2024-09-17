using Application.DTO.GetUserProfileDTO;
using Application.DTO.GroupDTO;
using Application.DTO.GroupPostDTO;
using Application.DTO.ReactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupPostByGroupPostId
{
    public class GetGroupPostByGroupPostIdResult
    {
        public Guid GroupPostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public string? GroupPostNumber { get; set; }
        public GetGroupStatusDTO? GroupStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? GroupPhotoId { get; set; }
        public Guid? GroupVideoId { get; set; }
        public int? NumberPost { get; set; }
        public bool? IsBanned { get; set; }
        public virtual GroupPhotoDTO? GroupPhoto { get; set; }
        public virtual GroupVideoDTO? GroupVideo { get; set; }
        public virtual ICollection<GroupPostPhotoDTO>? GroupPostPhoto { get; set; }
        public virtual ICollection<GroupPostVideoDTO>? GroupPostVideo { get; set; }
        public virtual GetUserAvatar UserAvata { get; set; } = null!;
        public string? UserName { get; set; }
        public Guid? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCorverImage { get; set; }
        public ReactCount? ReactCount { get; set; }
    }
}
