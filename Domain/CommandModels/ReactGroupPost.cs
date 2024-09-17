using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReactGroupPost
    {
        public Guid ReactGroupPostId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual GroupPost GroupPost { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
