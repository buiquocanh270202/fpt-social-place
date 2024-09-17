using API.Hub;
using Application.DTO.NotificationDTO;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Hub
{
    public interface INotificationsClient
    {
        
        Task ReceiveNotification(string message);
        Task listReceiveNotification(string message);
        Task ReceiveNotification(NotificationOutDTO message);
        Task listReceiveNotification(NotificationOutDTO message);
        Task ReceiveNotification(List<Notification> message);
        Task ReceiveNotification(string message, string url);
        Task ReceiveNotification(string firstMessage, string secondMessage, string url);
        
    }
}
