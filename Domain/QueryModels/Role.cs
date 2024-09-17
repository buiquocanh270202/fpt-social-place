using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class Role
    {
        public Role()
        {
            AdminProfiles = new HashSet<AdminProfile>();
            UserProfiles = new HashSet<UserProfile>();
        }

        public Guid RoleId { get; set; }
        public string NameRole { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<AdminProfile> AdminProfiles { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
