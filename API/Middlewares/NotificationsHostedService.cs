using API.Hub.SubscribeSqlTableDependencies;
using Domain.Extensions;
using System.Configuration;

namespace API.Middlewares
{
    public class NotificationsHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly SplitString _splitString;
        public NotificationsHostedService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _splitString = new SplitString();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            string connectionString = _splitString.SplitStringAfterForConnectString(_configuration.GetSection("ConnectionStrings").GetSection("CommandConnection").Value).First();
            using (var scope = _serviceProvider.CreateScope())
            {
                var notificationsTableDependency = scope.ServiceProvider.GetRequiredService<SubscribeNotificationsTableDependency>();
                notificationsTableDependency.SubscribeTableDependency(connectionString); // Ensure this method exists
                notificationsTableDependency.SubscribeBlockUserProfileTableDependency(connectionString);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
