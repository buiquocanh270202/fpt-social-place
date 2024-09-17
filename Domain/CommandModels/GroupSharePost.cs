using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupSharePost
    {
        public GroupSharePost()
        {
            CommentGroupSharePosts = new HashSet<CommentGroupSharePost>();
            ReactGroupSharePostComments = new HashSet<ReactGroupSharePostComment>();
            ReactGroupSharePosts = new HashSet<ReactGroupSharePost>();
            ReportPosts = new HashSet<ReportPost>();
        }

        public Guid GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? SharedToUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsHide { get; set; }
        public Guid? GroupStatusId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsBanned { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? UserSharedId { get; set; }
        public bool? IsPending { get; set; }

        public virtual GroupFpt? Group { get; set; }
        public virtual GroupPost? GroupPost { get; set; }
        public virtual GroupPostPhoto? GroupPostPhoto { get; set; }
        public virtual GroupPostVideo? GroupPostVideo { get; set; }
        public virtual GroupStatus? GroupStatus { get; set; }
        public virtual UserProfile? SharedToUser { get; set; }
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPost? UserPost { get; set; }
        public virtual UserPostPhoto? UserPostPhoto { get; set; }
        public virtual UserPostVideo? UserPostVideo { get; set; }
        public virtual UserProfile? UserShared { get; set; }
        public virtual ICollection<CommentGroupSharePost> CommentGroupSharePosts { get; set; }
        public virtual ICollection<ReactGroupSharePostComment> ReactGroupSharePostComments { get; set; }
        public virtual ICollection<ReactGroupSharePost> ReactGroupSharePosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
    }
}
