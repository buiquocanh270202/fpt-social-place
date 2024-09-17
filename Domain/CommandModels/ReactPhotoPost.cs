using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReactPhotoPost
    {
        public Guid ReactPhotoPostId { get; set; }
        public Guid UserPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPostPhoto UserPostPhoto { get; set; } = null!;
    }
}
