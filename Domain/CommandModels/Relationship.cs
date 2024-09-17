using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class Relationship
    {
        public Relationship()
        {
            UserRelationships = new HashSet<UserRelationship>();
        }

        public Guid RelationshipId { get; set; }
        public string RelationshipName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<UserRelationship> UserRelationships { get; set; }
    }
}
