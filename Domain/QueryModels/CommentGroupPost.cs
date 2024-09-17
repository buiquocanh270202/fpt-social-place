using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class CommentGroupPost
    {
        public CommentGroupPost()
        {
            ReactGroupCommentPosts = new HashSet<ReactGroupCommentPost>();
            ReportComments = new HashSet<ReportComment>();
        }

        public Guid CommentGroupPostId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? ListNumber { get; set; }
        public int? LevelCmt { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsBanned { get; set; }

        public virtual GroupPost GroupPost { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual ICollection<ReactGroupCommentPost> ReactGroupCommentPosts { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
    }
}
