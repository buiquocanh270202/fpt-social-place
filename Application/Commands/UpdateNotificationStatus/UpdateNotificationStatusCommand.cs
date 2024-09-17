using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateNotificationStatus
{
    public class UpdateNotificationStatusCommand : ICommand<UpdateNotificationStatusResult>
    {
        public Guid NotificationId { get; set; }
    }
}
