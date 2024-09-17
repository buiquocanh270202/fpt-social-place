using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReactVideoPostComment
    {
        public Guid ReactVideoPostCommentId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentVideoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual CommentVideoPost CommentVideoPost { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPostVideo UserPostVideo { get; set; } = null!;
    }
}
