using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReactGroupPhotoPost
    {
        public Guid ReactGroupPhotoPostId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual GroupPostPhoto GroupPostPhoto { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
