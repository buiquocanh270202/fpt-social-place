using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class CommentPost
    {
        public CommentPost()
        {
            ReactComments = new HashSet<ReactComment>();
            ReportComments = new HashSet<ReportComment>();
        }

        public Guid CommentId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? ListNumber { get; set; }
        public int? LevelCmt { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsBanned { get; set; }

        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPost UserPost { get; set; } = null!;
        public virtual ICollection<ReactComment> ReactComments { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
    }
}
