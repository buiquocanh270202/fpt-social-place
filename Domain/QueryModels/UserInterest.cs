using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class UserInterest
    {
        public Guid UserInterestId { get; set; }
        public Guid InterestId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Interest Interest { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
    }
}
