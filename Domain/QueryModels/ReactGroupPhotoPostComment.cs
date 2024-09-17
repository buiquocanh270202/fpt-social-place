using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class ReactGroupPhotoPostComment
    {
        public Guid ReactPhotoPostCommentId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentPhotoGroupPostId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual CommentPhotoGroupPost CommentPhotoGroupPost { get; set; } = null!;
        public virtual GroupPostPhoto GroupPostPhoto { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
