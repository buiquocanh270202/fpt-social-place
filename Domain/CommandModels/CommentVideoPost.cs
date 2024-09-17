using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class CommentVideoPost
    {
        public CommentVideoPost()
        {
            ReactVideoPostComments = new HashSet<ReactVideoPostComment>();
            ReportComments = new HashSet<ReportComment>();
        }

        public Guid CommentVideoPostId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? ListNumber { get; set; }
        public int? LevelCmt { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsBanned { get; set; }

        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPostVideo UserPostVideo { get; set; } = null!;
        public virtual ICollection<ReactVideoPostComment> ReactVideoPostComments { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
    }
}
