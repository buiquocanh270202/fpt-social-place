using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class ReactGroupCommentPost
    {
        public Guid ReactGroupCommentPostId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentGroupPostId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual CommentGroupPost CommentGroupPost { get; set; } = null!;
        public virtual GroupPost GroupPost { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
