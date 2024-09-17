using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class ReactGroupSharePostComment
    {
        public Guid ReactGroupSharePosCommentId { get; set; }
        public Guid GroupSharePostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public Guid? CommentGroupSharePostId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual CommentGroupSharePost? CommentGroupSharePost { get; set; }
        public virtual GroupSharePost GroupSharePost { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
