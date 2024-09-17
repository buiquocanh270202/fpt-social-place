using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class UserClientPermission
    {
        public int ClientId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateAt { get; set; }
        public Guid? AdminId { get; set; }

        public virtual AdminProfile? Admin { get; set; }
        public virtual Client Client { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
