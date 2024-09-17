using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class ReactGroupVideoPost
    {
        public Guid ReactGroupVideoPostId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual GroupPostVideo GroupPostVideo { get; set; } = null!;
        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
