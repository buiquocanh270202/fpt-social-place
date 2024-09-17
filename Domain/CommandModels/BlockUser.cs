using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class BlockUser
    {
        public Guid BlockUserId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserIsBlockedId { get; set; }
        public Guid? BlockTypeId { get; set; }
        public bool? IsBlock { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual BlockType? BlockType { get; set; }
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserProfile UserIsBlocked { get; set; } = null!;
    }
}
