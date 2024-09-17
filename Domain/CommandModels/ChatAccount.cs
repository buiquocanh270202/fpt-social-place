using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ChatAccount
    {
        public int ChatAccountId { get; set; }
        public string AccountId { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual UserProfile User { get; set; } = null!;
    }
}
