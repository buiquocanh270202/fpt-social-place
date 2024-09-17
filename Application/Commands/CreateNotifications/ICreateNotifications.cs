using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateNotifications
{
    public interface ICreateNotifications
    {
        public Task CreateNotitfication(string senderId, string receiverId, string notiMessage, string notifiUrl, [Optional] NotificationsTypeEnum? notificationsTypeEnum);
    }
}
