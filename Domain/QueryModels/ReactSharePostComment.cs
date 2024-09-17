using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class ReactSharePostComment
    {
        public Guid ReactSharePosCommentId { get; set; }
        public Guid SharePostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public Guid? CommentSharePostId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual CommentSharePost? CommentSharePost { get; set; }
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual SharePost SharePost { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
