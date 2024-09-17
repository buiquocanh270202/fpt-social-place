using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class UserPostVideo
    {
        public UserPostVideo()
        {
            CommentVideoPosts = new HashSet<CommentVideoPost>();
            GroupSharePosts = new HashSet<GroupSharePost>();
            PostReactCounts = new HashSet<PostReactCount>();
            ReactVideoPostComments = new HashSet<ReactVideoPostComment>();
            ReactVideoPosts = new HashSet<ReactVideoPost>();
            ReportPosts = new HashSet<ReportPost>();
            SharePosts = new HashSet<SharePost>();
        }

        public Guid UserPostVideoId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid VideoId { get; set; }
        public string? Content { get; set; }
        public string? UserPostVideoNumber { get; set; }
        public Guid UserStatusId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public bool? IsBanned { get; set; }

        public virtual UserPost UserPost { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
        public virtual Video Video { get; set; } = null!;
        public virtual ICollection<CommentVideoPost> CommentVideoPosts { get; set; }
        public virtual ICollection<GroupSharePost> GroupSharePosts { get; set; }
        public virtual ICollection<PostReactCount> PostReactCounts { get; set; }
        public virtual ICollection<ReactVideoPostComment> ReactVideoPostComments { get; set; }
        public virtual ICollection<ReactVideoPost> ReactVideoPosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
        public virtual ICollection<SharePost> SharePosts { get; set; }
    }
}
