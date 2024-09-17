using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class Friend
    {
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public bool Confirm { get; set; }
        public int? ReactCount { get; set; }
        public DateTime? LastInteractionDate { get; set; }

        public virtual UserProfile FriendNavigation { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
    }
}
