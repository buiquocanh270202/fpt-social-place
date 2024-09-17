using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class SharePost
    {
        public SharePost()
        {
            CommentSharePosts = new HashSet<CommentSharePost>();
            ReactSharePostComments = new HashSet<ReactSharePostComment>();
            ReactSharePosts = new HashSet<ReactSharePost>();
            ReportPosts = new HashSet<ReportPost>();
        }

        public Guid SharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? SharedToUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UserStatusId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsBanned { get; set; }
        public Guid? UserSharedId { get; set; }

        public virtual GroupPost? GroupPost { get; set; }
        public virtual GroupPostPhoto? GroupPostPhoto { get; set; }
        public virtual GroupPostVideo? GroupPostVideo { get; set; }
        public virtual UserProfile? SharedToUser { get; set; }
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPost? UserPost { get; set; }
        public virtual UserPostPhoto? UserPostPhoto { get; set; }
        public virtual UserPostVideo? UserPostVideo { get; set; }
        public virtual UserProfile? UserShared { get; set; }
        public virtual UserStatus? UserStatus { get; set; }
        public virtual ICollection<CommentSharePost> CommentSharePosts { get; set; }
        public virtual ICollection<ReactSharePostComment> ReactSharePostComments { get; set; }
        public virtual ICollection<ReactSharePost> ReactSharePosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
    }
}
