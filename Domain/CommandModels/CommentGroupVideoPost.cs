using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class CommentGroupVideoPost
    {
        public CommentGroupVideoPost()
        {
            ReactGroupVideoPostComments = new HashSet<ReactGroupVideoPostComment>();
            ReportComments = new HashSet<ReportComment>();
        }

        public Guid CommentGroupVideoPostId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? ListNumber { get; set; }
        public int? LevelCmt { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsBanned { get; set; }

        public virtual GroupPostVideo GroupPostVideo { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual ICollection<ReactGroupVideoPostComment> ReactGroupVideoPostComments { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
    }
}
