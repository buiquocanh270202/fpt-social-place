using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReactComment
    {
        public Guid ReactCommentId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual CommentPost Comment { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPost UserPost { get; set; } = null!;
    }
}
