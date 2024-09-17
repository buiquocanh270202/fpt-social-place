using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReportComment
    {
        public Guid ReportCommentId { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid? CommentId { get; set; }
        public Guid? CommentPhotoPostId { get; set; }
        public Guid? CommentVideoPostId { get; set; }
        public Guid? CommentGroupPostId { get; set; }
        public Guid? CommentPhotoGroupPostId { get; set; }
        public Guid? CommentGroupVideoPostId { get; set; }
        public Guid? CommentSharePostId { get; set; }
        public Guid? CommentGroupSharePostId { get; set; }
        public string? Content { get; set; }
        public Guid ReportById { get; set; }
        public bool? ReportStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Processing { get; set; }

        public virtual CommentPost? Comment { get; set; }
        public virtual CommentGroupPost? CommentGroupPost { get; set; }
        public virtual CommentGroupSharePost? CommentGroupSharePost { get; set; }
        public virtual CommentGroupVideoPost? CommentGroupVideoPost { get; set; }
        public virtual CommentPhotoGroupPost? CommentPhotoGroupPost { get; set; }
        public virtual CommentPhotoPost? CommentPhotoPost { get; set; }
        public virtual CommentSharePost? CommentSharePost { get; set; }
        public virtual CommentVideoPost? CommentVideoPost { get; set; }
        public virtual UserProfile ReportBy { get; set; } = null!;
        public virtual ReportType ReportType { get; set; } = null!;
    }
}
