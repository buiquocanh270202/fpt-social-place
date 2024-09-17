using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class AdminProfile
    {
        public Guid AdminId { get; set; }
        public Guid RoleId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
