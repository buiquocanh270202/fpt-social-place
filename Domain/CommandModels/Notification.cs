using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public Guid SenderId { get; set; }
        public Guid NotificationTypeId { get; set; }
        public string? NotiMessage { get; set; }
        public Guid UserStatusId { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? NotifiUrl { get; set; }

        public virtual NotificationType NotificationType { get; set; } = null!;
        public virtual UserProfile Sender { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
    }
}
