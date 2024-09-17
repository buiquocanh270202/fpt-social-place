using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class UserChat
    {
        public int UserChatId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ChatWithId { get; set; }

        public virtual UserProfile? ChatWith { get; set; }
        public virtual UserProfile User { get; set; } = null!;
    }
}
