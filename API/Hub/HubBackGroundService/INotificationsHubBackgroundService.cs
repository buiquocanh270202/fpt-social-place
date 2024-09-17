using Microsoft.AspNetCore.SignalR;

namespace API.Hub
{
    public interface INotificationsHubBackgroundService
    {
        public Task SendNotifyService(HubCallerContext context, string notice);
        public Task SendGroupNotifyService(HubCallerContext context, string notice);
        public Task PushAllNotifyByUserIdWithTableDependencyService(HubCallerContext context, string userId);
    }
}
