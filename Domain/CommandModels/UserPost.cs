using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class UserPost
    {
        public UserPost()
        {
            CommentPosts = new HashSet<CommentPost>();
            GroupSharePosts = new HashSet<GroupSharePost>();
            PostReactCounts = new HashSet<PostReactCount>();
            ReactComments = new HashSet<ReactComment>();
            ReactPosts = new HashSet<ReactPost>();
            ReportPosts = new HashSet<ReportPost>();
            SharePosts = new HashSet<SharePost>();
            UserPostPhotos = new HashSet<UserPostPhoto>();
            UserPostVideos = new HashSet<UserPostVideo>();
        }

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
        public bool? IsBanned { get; set; }

        public virtual Photo? Photo { get; set; }
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
        public virtual Video? Video { get; set; }
        public virtual ICollection<CommentPost> CommentPosts { get; set; }
        public virtual ICollection<GroupSharePost> GroupSharePosts { get; set; }
        public virtual ICollection<PostReactCount> PostReactCounts { get; set; }
        public virtual ICollection<ReactComment> ReactComments { get; set; }
        public virtual ICollection<ReactPost> ReactPosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
        public virtual ICollection<SharePost> SharePosts { get; set; }
        public virtual ICollection<UserPostPhoto> UserPostPhotos { get; set; }
        public virtual ICollection<UserPostVideo> UserPostVideos { get; set; }
    }
}
