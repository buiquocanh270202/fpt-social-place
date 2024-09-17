using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReactPhotoPostComment
    {
        public Guid ReactPhotoPostCommentId { get; set; }
        public Guid UserPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentPhotoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual CommentPhotoPost CommentPhotoPost { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPostPhoto UserPostPhoto { get; set; } = null!;
    }
}
