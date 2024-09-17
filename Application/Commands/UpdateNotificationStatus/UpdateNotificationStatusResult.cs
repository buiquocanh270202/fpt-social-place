using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateNotificationStatus
{
    public class UpdateNotificationStatusResult
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
    }
}
