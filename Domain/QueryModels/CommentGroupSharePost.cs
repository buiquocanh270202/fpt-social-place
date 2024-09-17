using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class CommentGroupSharePost
    {
        public CommentGroupSharePost()
        {
            ReactGroupSharePostComments = new HashSet<ReactGroupSharePostComment>();
            ReportComments = new HashSet<ReportComment>();
        }

        public Guid CommentGroupSharePostId { get; set; }
        public Guid GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? ListNumber { get; set; }
        public int? LevelCmt { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsBanned { get; set; }

        public virtual GroupSharePost GroupSharePost { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual ICollection<ReactGroupSharePostComment> ReactGroupSharePostComments { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
    }
}
