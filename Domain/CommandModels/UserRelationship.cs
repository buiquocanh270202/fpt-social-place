using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class UserRelationship
    {
        public Guid UserRelationshipId { get; set; }
        public Guid RelationshipId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Relationship Relationship { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
    }
}
