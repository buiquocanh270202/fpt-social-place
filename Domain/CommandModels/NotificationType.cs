using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notifications = new HashSet<Notification>();
        }

        public Guid NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
