using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class GroupInvitation
    {
        public Guid InvitationId { get; set; }
        public Guid GroupId { get; set; }
        public Guid InviterId { get; set; }
        public Guid InvitedId { get; set; }
        public bool StatusAccept { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual GroupFpt Group { get; set; } = null!;
        public virtual UserProfile Invited { get; set; } = null!;
        public virtual UserProfile Inviter { get; set; } = null!;
    }
}
