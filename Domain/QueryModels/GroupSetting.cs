using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class GroupSetting
    {
        public GroupSetting()
        {
            GroupSettingUses = new HashSet<GroupSettingUse>();
        }

        public Guid GroupSettingId { get; set; }
        public string GroupSettingName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<GroupSettingUse> GroupSettingUses { get; set; }
    }
}
