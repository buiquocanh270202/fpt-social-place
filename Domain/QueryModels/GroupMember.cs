using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class GroupMember
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public Guid GroupRoleId { get; set; }
        public bool IsJoined { get; set; }
        public DateTime? JoinedDate { get; set; }
        public bool? IsInvated { get; set; }
        public Guid? InvatedBy { get; set; }

        public virtual GroupFpt Group { get; set; } = null!;
        public virtual GroupRole GroupRole { get; set; } = null!;
        public virtual UserProfile? InvatedByNavigation { get; set; }
        public virtual UserProfile User { get; set; } = null!;
    }
}
