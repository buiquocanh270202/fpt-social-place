using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class GroupRole
    {
        public GroupRole()
        {
            GroupMembers = new HashSet<GroupMember>();
        }

        public Guid GroupRoleId { get; set; }
        public string GroupRoleName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<GroupMember> GroupMembers { get; set; }
    }
}
