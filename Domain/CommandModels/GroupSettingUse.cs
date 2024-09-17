using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupSettingUse
    {
        public Guid GroupId { get; set; }
        public Guid GroupSettingId { get; set; }
        public Guid GroupStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual GroupFpt Group { get; set; } = null!;
        public virtual GroupSetting GroupSetting { get; set; } = null!;
        public virtual GroupStatus GroupStatus { get; set; } = null!;
    }
}
