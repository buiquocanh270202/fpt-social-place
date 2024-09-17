using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReactPost
    {
        public Guid ReactPostId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ReactType ReactType { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserPost UserPost { get; set; } = null!;
    }
}
