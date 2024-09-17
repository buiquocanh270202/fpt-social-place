using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.NotificationDTO
{
    public class NotificationOutDTO
    {
        public Guid NotificationId { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        //public string ReciverId { get; set; }
        public string SenderAvatar { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetAvatarSenderDTO
    {
        public UserProfile UserProfile { get; set; }
        public string SenderAvatarURL { get; set; }
    }
}
