using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class GroupPostVideo
    {
        public GroupPostVideo()
        {
            CommentGroupVideoPosts = new HashSet<CommentGroupVideoPost>();
            GroupPostReactCounts = new HashSet<GroupPostReactCount>();
            GroupSharePosts = new HashSet<GroupSharePost>();
            ReactGroupVideoPostComments = new HashSet<ReactGroupVideoPostComment>();
            ReactGroupVideoPosts = new HashSet<ReactGroupVideoPost>();
            ReportPosts = new HashSet<ReportPost>();
            SharePosts = new HashSet<SharePost>();
        }

        public Guid GroupPostVideoId { get; set; }
        public Guid GroupPostId { get; set; }
        public string? Content { get; set; }
        public Guid GroupVideoId { get; set; }
        public Guid GroupStatusId { get; set; }
        public string? GroupPostVideoNumber { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public bool? IsBanned { get; set; }
        public Guid? GroupId { get; set; }
        public bool? IsPending { get; set; }

        public virtual GroupFpt? Group { get; set; }
        public virtual GroupPost GroupPost { get; set; } = null!;
        public virtual GroupStatus GroupStatus { get; set; } = null!;
        public virtual GroupVideo GroupVideo { get; set; } = null!;
        public virtual ICollection<CommentGroupVideoPost> CommentGroupVideoPosts { get; set; }
        public virtual ICollection<GroupPostReactCount> GroupPostReactCounts { get; set; }
        public virtual ICollection<GroupSharePost> GroupSharePosts { get; set; }
        public virtual ICollection<ReactGroupVideoPostComment> ReactGroupVideoPostComments { get; set; }
        public virtual ICollection<ReactGroupVideoPost> ReactGroupVideoPosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
        public virtual ICollection<SharePost> SharePosts { get; set; }
    }
}
