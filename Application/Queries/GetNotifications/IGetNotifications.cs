using Application.DTO.NotificationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetNotifications
{
    public interface IGetNotifications
    {
        public List<Domain.QueryModels.GroupMember> GetGroupMemberByGroupId(string groupId);
        public GetAvatarSenderDTO GetAvatarBySenderId(string senderId);
        public Domain.QueryModels.UserProfile GetUserProfileByUserId(string userId);
        public List<Domain.QueryModels.Notification> GetNotifyByUserId(string userId);
        public List<NotificationOutDTO> GetNotifyByUserIds(string userId);
        public List<Domain.QueryModels.Notification> GetNotifyBySenderId(string senderId);
        public bool IsNotifyExist(string senderId, string msgDB);
    }
}
