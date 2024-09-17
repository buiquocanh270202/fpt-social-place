using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class CommentPhotoGroupPost
    {
        public CommentPhotoGroupPost()
        {
            ReactGroupPhotoPostComments = new HashSet<ReactGroupPhotoPostComment>();
            ReportComments = new HashSet<ReportComment>();
        }

        public Guid CommentPhotoGroupPostId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? ListNumber { get; set; }
        public int? LevelCmt { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsBanned { get; set; }

        public virtual GroupPostPhoto GroupPostPhoto { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual ICollection<ReactGroupPhotoPostComment> ReactGroupPhotoPostComments { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
    }
}
