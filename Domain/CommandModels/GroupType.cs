using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupType
    {
        public GroupType()
        {
            GroupFpts = new HashSet<GroupFpt>();
        }

        public Guid GroupTypeId { get; set; }
        public string GroupTypeName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<GroupFpt> GroupFpts { get; set; }
    }
}
