using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class WebAffiliation
    {
        public Guid WebAffiliationId { get; set; }
        public string WebAffiliationUrl { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
    }
}
