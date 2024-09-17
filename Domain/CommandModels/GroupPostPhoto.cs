using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupPostPhoto
    {
        public GroupPostPhoto()
        {
            CommentPhotoGroupPosts = new HashSet<CommentPhotoGroupPost>();
            GroupPostReactCounts = new HashSet<GroupPostReactCount>();
            GroupSharePosts = new HashSet<GroupSharePost>();
            ReactGroupPhotoPostComments = new HashSet<ReactGroupPhotoPostComment>();
            ReactGroupPhotoPosts = new HashSet<ReactGroupPhotoPost>();
            ReportPosts = new HashSet<ReportPost>();
            SharePosts = new HashSet<SharePost>();
        }

        public Guid GroupPostPhotoId { get; set; }
        public Guid GroupPostId { get; set; }
        public string? Content { get; set; }
        public Guid GroupPhotoId { get; set; }
        public Guid GroupStatusId { get; set; }
        public string? GroupPostPhotoNumber { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public bool? IsBanned { get; set; }
        public Guid? GroupId { get; set; }
        public bool? IsPending { get; set; }

        public virtual GroupFpt? Group { get; set; }
        public virtual GroupPhoto GroupPhoto { get; set; } = null!;
        public virtual GroupPost GroupPost { get; set; } = null!;
        public virtual GroupStatus GroupStatus { get; set; } = null!;
        public virtual ICollection<CommentPhotoGroupPost> CommentPhotoGroupPosts { get; set; }
        public virtual ICollection<GroupPostReactCount> GroupPostReactCounts { get; set; }
        public virtual ICollection<GroupSharePost> GroupSharePosts { get; set; }
        public virtual ICollection<ReactGroupPhotoPostComment> ReactGroupPhotoPostComments { get; set; }
        public virtual ICollection<ReactGroupPhotoPost> ReactGroupPhotoPosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
        public virtual ICollection<SharePost> SharePosts { get; set; }
    }
}
