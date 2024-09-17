using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReactGroupVideoPostComment
    {
        public Guid ReactGroupVideoCommentId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentGroupVideoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual CommentGroupVideoPost CommentGroupVideoPost { get; set; } = null!;
        public virtual GroupPostVideo GroupPostVideo { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
