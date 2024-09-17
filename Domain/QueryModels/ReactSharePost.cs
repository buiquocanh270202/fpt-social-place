using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class ReactSharePost
    {
        public Guid ReactSharePostId { get; set; }
        public Guid SharePostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ReactType ReactType { get; set; } = null!;
        public virtual SharePost SharePost { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
