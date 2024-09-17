using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class CommentSharePost
    {
        public CommentSharePost()
        {
            ReactSharePostComments = new HashSet<ReactSharePostComment>();
            ReportComments = new HashSet<ReportComment>();
        }

        public Guid CommentSharePostId { get; set; }
        public Guid SharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? ListNumber { get; set; }
        public int? LevelCmt { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsBanned { get; set; }

        public virtual SharePost SharePost { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual ICollection<ReactSharePostComment> ReactSharePostComments { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
    }
}
