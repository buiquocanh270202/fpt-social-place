using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class AvataPhoto
    {
        public Guid AvataPhotosId { get; set; }
        public string AvataPhotosUrl { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
    }
}
