using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Application.Commands.CreateNotifications
{
    public enum NotificationsTypeEnum
    {
        NORMAL,
        IMPORTANCE
    }


    public class CreateNotifications : ICreateNotifications
    {
        const string NORMAL = "Normal";
        private readonly IGuidHelper _helper;
        private readonly IServiceProvider _serviceProvider;
        public fptforumCommandContext _commandContext;
        public fptforumQueryContext _queryContext;
        public CreateNotifications(IServiceProvider serviceProvider)
        {
            _helper = new GuidHelper();
            _serviceProvider = serviceProvider;
            _commandContext = new();
            _queryContext = new();
        }
        public async Task CreateNotitfication(string senderId, string receiverId, string notiMessage, string notifiUrl, [Optional] NotificationsTypeEnum? notificationsTypeEnum)
        {
            //string notiType;
            //switch (notificationsTypeEnum)
            //{
            //    case NotificationsTypeEnum.NORMAL:
            //        notiType = "Normal";
            //        break;
            //    case NotificationsTypeEnum.IMPORTANCE:
            //        notiType = "Importance";
            //        break;
            //    default:
            //        notiType = "Normal";
            //        break;
            //}


            Domain.CommandModels.Notification _notification = new();

            if (_queryContext == null || _commandContext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            Domain.QueryModels.UserProfile user = await _queryContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId.Equals(Guid.Parse(receiverId)));
            if (user == null)
            {
                throw new ErrorException(StatusCodeEnum.Error);
            }

            if (notificationsTypeEnum != null)
            {

                Domain.QueryModels.NotificationType receiverTypeE = await _queryContext.NotificationTypes.FirstOrDefaultAsync(x => x.NotificationTypeName.Equals(notificationsTypeEnum));
                if (receiverTypeE == null)
                {
                    throw new ErrorException(StatusCodeEnum.NT01_Not_Found);
                }
                _notification.NotificationTypeId = receiverTypeE.NotificationTypeId;
            }
            if (notificationsTypeEnum == null)
            {
                Domain.QueryModels.NotificationType receiverTypeE = await _queryContext.NotificationTypes.FirstOrDefaultAsync(x => x.NotificationTypeName.Equals(NORMAL));
                if (receiverTypeE == null)
                {
                    throw new ErrorException(StatusCodeEnum.NT01_Not_Found);
                }
                _notification.NotificationTypeId = receiverTypeE.NotificationTypeId;
            }
            _notification.NotificationId = _helper.GenerateNewGuid();
            _notification.UserId = user.UserId;
            _notification.SenderId = Guid.Parse(senderId);
            _notification.NotiMessage = notiMessage;
            _notification.UserStatusId = user.UserStatusId;
            _notification.IsRead = false;
            _notification.CreatedAt = DateTime.Now;
            _notification.NotifiUrl = notifiUrl;

            _commandContext.Notifications.Add(_notification);
            await _commandContext.SaveChangesAsync();

        }

    }
}
