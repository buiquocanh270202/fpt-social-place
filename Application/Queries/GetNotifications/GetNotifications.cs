using Application.DTO.NotificationDTO;
using AutoMapper;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Queries.GetNotifications
{
    public class GetNotifications : IGetNotifications
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly SplitString _splitString;
        public GetNotifications(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _splitString = new SplitString();
        }

        public Domain.QueryModels.UserProfile GetUserProfileByUserId(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _querycontext = scope.ServiceProvider.GetRequiredService<fptforumQueryContext>();
                if (_querycontext == null)
                {
                    throw new ErrorException(StatusCodeEnum.Context_Not_Found);
                }
                var user = _querycontext.UserProfiles.FirstOrDefault(x => x.UserId.Equals(Guid.Parse(userId)));

                if (user == null)
                {
                    throw new ErrorException(StatusCodeEnum.U01_Not_Found);
                }
                return user;
            }

        }
        public GetAvatarSenderDTO GetAvatarBySenderId(string senderId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                GetAvatarSenderDTO getAvatarSenderDTO = new();
                var _querycontext = scope.ServiceProvider.GetRequiredService<fptforumQueryContext>();
                //if (_querycontext == null)
                //{
                //    throw new ErrorException(StatusCodeEnum.Context_Not_Found);
                //}
                var user = _querycontext.UserProfiles.FirstOrDefault(x => x.UserId.Equals(Guid.Parse(senderId)));
                var avatarUrl = _querycontext.AvataPhotos.FirstOrDefault(x => x.UserId.Equals(Guid.Parse(senderId)) && x.IsUsed == true);
                if (avatarUrl == null)
                {
                    getAvatarSenderDTO.UserProfile = user;
                    getAvatarSenderDTO.SenderAvatarURL = "";
                }
                else
                {
                    getAvatarSenderDTO.UserProfile = user;
                    getAvatarSenderDTO.SenderAvatarURL = avatarUrl.AvataPhotosUrl;
                }


                return getAvatarSenderDTO;
            }

        }

        public List<Domain.QueryModels.Notification> GetNotifyByUserId(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _querycontext = scope.ServiceProvider.GetRequiredService<fptforumQueryContext>();

                if (_querycontext == null)
                {
                    throw new ErrorException(StatusCodeEnum.Context_Not_Found);
                }
                var notice = _querycontext.Notifications.Where(x => x.UserId == Guid.Parse(userId))
                                                        .OrderByDescending(x => x.CreatedAt)
                                                        .Take(10)
                                                        .ToList();
                return notice;
            }
        }

        public List<NotificationOutDTO> GetNotifyByUserIds(string userId)
        {
            //Chua check dc trg hop avatar isUse(Vi co truong hop nguoi dung ko co avatar => ko co isUsed send noti)
            using (var scope = _serviceProvider.CreateScope())
            {
                var _querycontext = scope.ServiceProvider.GetRequiredService<fptforumQueryContext>();

                if (_querycontext == null)
                {
                    throw new ErrorException(StatusCodeEnum.Context_Not_Found);
                }

                var userIdGuid = Guid.Parse(userId);

                var notifications = (from n in _querycontext.Notifications
                                     join s in _querycontext.UserProfiles on n.SenderId equals s.UserId
                                     where n.UserId == userIdGuid && n.UserId != n.SenderId
                                     orderby n.CreatedAt descending
                                     select new
                                     {
                                         n.NotificationId,
                                         n.SenderId,
                                         SenderName = s.FirstName + " " + s.LastName,
                                         n.NotiMessage,
                                         n.IsRead,
                                         n.NotifiUrl,
                                         n.CreatedAt,
                                         SenderAvatar = (from a in _querycontext.AvataPhotos
                                                         where a.UserId == n.SenderId && a.IsUsed
                                                         orderby a.CreatedAt descending
                                                         select a.AvataPhotosUrl).FirstOrDefault()
                                     }).Take(10).AsEnumerable() // Convert to in-memory collection to allow for nullable string property assignment
                                     .Select(n => new NotificationOutDTO
                                     {
                                         NotificationId = n.NotificationId,
                                         SenderId = n.SenderId.ToString(),
                                         SenderName = n.SenderName,
                                         SenderAvatar = n.SenderAvatar ?? string.Empty,
                                         Message = _splitString.SplitStringForNotify(n.NotiMessage).Last(),
                                         Url = n.NotifiUrl,
                                         IsRead = (bool)n.IsRead,
                                         CreatedAt = (DateTime)n.CreatedAt,
                                     }).ToList();


                return notifications;
            }
        }

        public bool IsNotifyExist(string senderId, string msgDB)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                bool result = false;
                var _querycontext = scope.ServiceProvider.GetRequiredService<fptforumQueryContext>();
                var sevenDaysAgo = DateTime.Now.AddDays(-7);
                if (_querycontext == null)
                {
                    throw new ErrorException(StatusCodeEnum.Context_Not_Found);
                }

                var latestNotify = _querycontext.Notifications
                                    .Where(x => x.SenderId == Guid.Parse(senderId) &&
                                            x.NotiMessage == msgDB &&
                                            x.IsRead == false &&
                                            x.CreatedAt >= sevenDaysAgo)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .FirstOrDefault();
                if (latestNotify != null)
                {
                    result = true;
                }
                return result;
            }
        }
        public List<Domain.QueryModels.Notification> GetNotifyBySenderId(string senderId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _querycontext = scope.ServiceProvider.GetRequiredService<fptforumQueryContext>();

                if (_querycontext == null)
                {
                    throw new ErrorException(StatusCodeEnum.Context_Not_Found);
                }
                var notice = _querycontext.Notifications.Where(x => x.SenderId.Equals(Guid.Parse(senderId))).ToList();
                return notice;
            }
        }

        public List<Domain.QueryModels.GroupMember> GetGroupMemberByGroupId(string groupId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _querycontext = scope.ServiceProvider.GetRequiredService<fptforumQueryContext>();
                //if (_querycontext == null)
                //{
                //    throw new ErrorException(StatusCodeEnum.Context_Not_Found);
                //}
                var user = _querycontext.GroupMembers.Where(x => x.GroupId.Equals(Guid.Parse(groupId)) && x.IsJoined == true).ToList();

                return user;
            }

        }

    }
}
