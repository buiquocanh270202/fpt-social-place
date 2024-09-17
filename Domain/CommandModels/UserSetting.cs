using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class UserSetting
    {
        public Guid UserSettingId { get; set; }
        public Guid SettingId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }

        public virtual Setting Setting { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
    }
}
