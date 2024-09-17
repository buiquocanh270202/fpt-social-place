using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    //public class ServerTimeNotifications : BackgroundService
    //{
    //    private static readonly TimeSpan Period = TimeSpan.FromSeconds(2); //--> Frequency To Send Noti
    //    private readonly ILogger<ServerTimeNotifications> _logger;
    //    private readonly IHubContext<NotificationsHub, INotificationsClient> _context;

    //    public ServerTimeNotifications(ILogger<ServerTimeNotifications> logger, IHubContext<NotificationsHub, INotificationsClient> context)
    //    {
    //        _logger = logger;
    //        _context = context;
    //    }

    //    protected override async task executeasync(cancellationtoken stoppingtoken)
    //    {
    //        using periodictimer timer = new periodictimer(period);
    //        while (!stoppingtoken.iscancellationrequested && await timer.waitfornexttickasync(stoppingtoken))
    //        {
    //            datetime datetime = datetime.now;
    //            _logger.loginformation($"excute {1}", nameof(servertimenotifications), datetime);

    //            await _context.clients.all.receivenotification($"server time = {datetime}");
    //        }

    //    }
    //}
}
