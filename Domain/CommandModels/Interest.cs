using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class Interest
    {
        public Interest()
        {
            UserInterests = new HashSet<UserInterest>();
        }

        public Guid InterestId { get; set; }
        public string InterestName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<UserInterest> UserInterests { get; set; }
    }
}
