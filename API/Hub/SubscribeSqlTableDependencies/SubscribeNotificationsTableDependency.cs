using Application.Hub;
using Domain.CommandModels;
using TableDependency.SqlClient;

namespace API.Hub.SubscribeSqlTableDependencies
{
    public class SubscribeNotificationsTableDependency : ISubscribeSqlTableDependency
    {
        SqlTableDependency<Notification> _notificationCommandTableDependency;
        SqlTableDependency<UserProfile> _blockUserProfileTableDependency;
        NotificationsHub _notificationsHub;
        NotificationsHubBackgroundService _notificationsHubBackgroundService;

        public SubscribeNotificationsTableDependency(NotificationsHub notificationsHub, NotificationsHubBackgroundService notificationsHubBackgroundService)
        {
            _notificationsHub = notificationsHub;
            _notificationsHubBackgroundService = notificationsHubBackgroundService;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            _notificationCommandTableDependency = new SqlTableDependency<Notification>(connectionString);
            _notificationCommandTableDependency.OnChanged += TableDependency_OnChanged;
            _notificationCommandTableDependency.OnError += TableDependency_OnError;
            _notificationCommandTableDependency.Start();
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Notification> e)
        {
            string userId = e.Entity.UserId.ToString();
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await _notificationsHub.PushAllNotifyByUserIdWithTableDependency(userId);
            }
        }
        public void SubscribeBlockUserProfileTableDependency(string connectionString)
        {
            _blockUserProfileTableDependency = new SqlTableDependency<UserProfile>(connectionString);
            _blockUserProfileTableDependency.OnChanged += BlockUserProfileTableDependency_OnChanged;
            _blockUserProfileTableDependency.OnError += TableDependency_OnError;
            _blockUserProfileTableDependency.Start();
        }

        private async void BlockUserProfileTableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<UserProfile> e)
        {
            if (!e.Entity.IsActive)
            {
                var isActive = e.Entity.IsActive;
                string userId = e.Entity.UserId.ToString();
                if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
                {
                    await _notificationsHubBackgroundService.SendEmailAsync(e.Entity.Email, isActive, e.Entity, false);
                }

            }

            if (e.Entity.IsActive && e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
            {
                var isActive = e.Entity.IsActive;
                string userId = e.Entity.UserId.ToString();
                if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
                {
                    await _notificationsHubBackgroundService.SendEmailAsync(e.Entity.Email, isActive, e.Entity, true);
                }
            }

            if (e.Entity.IsActive && e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update)
            {
                var isActive = e.Entity.IsActive;
                string userId = e.Entity.UserId.ToString();
                if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
                {
                    await _notificationsHubBackgroundService.SendEmailAsync(e.Entity.Email, isActive, e.Entity, false);
                }
            }

        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Notification)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}
