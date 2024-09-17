using Application.Commands.CreateNotifications;
using Application.DTO.NotificationDTO;
using Application.Hub;
using Application.Queries.GetNotifications;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;

namespace API.Hub
{
    public enum NotificationsTypeEnum
    {
        NORMAL,
        IMPORTANCE
    }

    public class NotificationsHubBackgroundService : BackgroundService, INotificationsHubBackgroundService
    {
        const string SEC = ")s%ec!r_e-t?^(";
        const string NORMAL = "Normal";
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(300);
        private readonly ConnectionMapping<string> _connections;
        private readonly ICreateNotifications _createNotifications;
        private readonly IGetNotifications _getNotifications;
        private readonly GuidHelper _helper;
        private readonly SplitString _splitString;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationsHub, INotificationsClient> _hubContext;
        private readonly HubCallerContext _hubCallerContext;
        private readonly SmtpClient _smtpClient;
        public NotificationsHubBackgroundService(IConfiguration configuration, IHubContext<NotificationsHub, INotificationsClient> hubContext,
            ICreateNotifications createNotifications, IGetNotifications getNotifications, ConnectionMapping<string> connections)
        {
            _helper = new GuidHelper();
            _configuration = configuration;
            _createNotifications = createNotifications;
            _getNotifications = getNotifications;
            _hubContext = hubContext;
            _connections = connections;
            _splitString = new SplitString();

            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // sử dụng TLS, nếu SSL thì dùng 465
                Credentials = new NetworkCredential("anhbqhe163864@fpt.edu.vn", "kpgc vtlr ucmn ofkh"),
                EnableSsl = true // Bật SSL hoặc TLS
            };
        }

        public async Task SendNotifyService(HubCallerContext context, string notice)
        {
            HttpContext _httpContext = context.GetHttpContext();
            dynamic routeOb = JsonConvert.DeserializeObject<dynamic>(notice);
            string receiverId = routeOb.Receiver;
            string url = routeOb.Url;
            string code = routeOb.MsgCode;
            string addMsg = routeOb.AddMsg;
            string actionId = routeOb.ActionId;
            string msgDB;

            if (_httpContext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            //var rawToken = _httpContext.Request.Query["access_token"];
            var rawToken = _httpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var path = _httpContext.Request.Path;
            if (string.IsNullOrEmpty(rawToken) &&
                (!path.StartsWithSegments("/notificationsHub")))
            {
                await _hubContext.Clients.Client(context.ConnectionId).ReceiveNotification($"The  ({context.ConnectionId}) Connected Fail!");
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                await _hubContext.Clients.Client(context.ConnectionId).ReceiveNotification($"The  ({context.ConnectionId}) Connected Fail!");
            }
            var senderId = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;
            if (senderId != receiverId)
            {
                var senderInfo = _getNotifications.GetAvatarBySenderId(senderId);

                string senderName = senderInfo.UserProfile.FirstName + " " + senderInfo.UserProfile.LastName;
                string notificationsMessage = _configuration.GetSection("MessageContents").GetSection(code).Value;
                if (actionId == null)
                {
                    msgDB = senderName + SEC + notificationsMessage + addMsg;
                    string msg = notificationsMessage + addMsg;
                    NotificationOutDTO notificationOutDTO = new()
                    {
                        SenderId = senderId,
                        SenderName = senderName,
                        //ReciverId = receiverId,
                        SenderAvatar = senderInfo.SenderAvatarURL,
                        Message = msg,
                        Url = url

                    };
                    string jsonNotice = System.Text.Json.JsonSerializer.Serialize(notificationOutDTO);

                    foreach (var connectionId in _connections.GetConnections(receiverId))
                    {
                        await _hubContext.Clients.Client(connectionId).ReceiveNotification(jsonNotice);
                    }

                    await _createNotifications.CreateNotitfication(senderId, receiverId, msgDB, url);
                }
                else
                {
                    msgDB = senderName + SEC + actionId + SEC + notificationsMessage + addMsg;
                    bool isExist = _getNotifications.IsNotifyExist(senderId, msgDB);
                    if (!isExist)
                    {
                        string msg = notificationsMessage + addMsg;
                        NotificationOutDTO notificationOutDTO = new()
                        {
                            SenderId = senderId,
                            SenderName = senderName,
                            //ReciverId = receiverId,
                            SenderAvatar = senderInfo.SenderAvatarURL,
                            Message = msg,
                            Url = url

                        };
                        string jsonNotice = System.Text.Json.JsonSerializer.Serialize(notificationOutDTO);

                        foreach (var connectionId in _connections.GetConnections(receiverId))
                        {
                            await _hubContext.Clients.Client(connectionId).ReceiveNotification(jsonNotice);
                        }

                        await _createNotifications.CreateNotitfication(senderId, receiverId, msgDB, url);
                    }

                }


            }
        }

        public async Task SendGroupNotifyService(HubCallerContext context, string notice)
        {
            HttpContext _httpContext = context.GetHttpContext();
            dynamic routeOb = JsonConvert.DeserializeObject<dynamic>(notice);
            string groupId = routeOb.Receiver;
            string url = routeOb.Url;
            string code = routeOb.MsgCode;
            string addMsg = routeOb.AddMsg;
            //Domain.QueryModels.UserProfile sender;

            if (_httpContext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            //var rawToken = _httpContext.Request.Query["access_token"];
            var rawToken = _httpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var path = _httpContext.Request.Path;
            if (string.IsNullOrEmpty(rawToken) &&
                (!path.StartsWithSegments("/notificationsHub")))
            {
                await _hubContext.Clients.Client(context.ConnectionId).ReceiveNotification($"The  ({context.ConnectionId}) Connected Fail!");
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                await _hubContext.Clients.Client(context.ConnectionId).ReceiveNotification($"The  ({context.ConnectionId}) Connected Fail!");
            }
            var senderId = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;



            var senderInfo = _getNotifications.GetAvatarBySenderId(senderId);
            var groupMember = _getNotifications.GetGroupMemberByGroupId(groupId);

            string senderName = senderInfo.UserProfile.FirstName + " " + senderInfo.UserProfile.LastName;
            string notificationsMessage = _configuration.GetSection("MessageContents").GetSection(code).Value;
            string msgDB = senderName + SEC + notificationsMessage + addMsg;
            string msg = notificationsMessage + addMsg;
            NotificationOutDTO notificationOutDTO = new()
            {
                SenderId = senderId,
                SenderName = senderName,
                //ReciverId = receiverId,
                SenderAvatar = senderInfo.SenderAvatarURL,
                Message = msg,
                Url = url

            };
            string jsonNotice = System.Text.Json.JsonSerializer.Serialize(notificationOutDTO);
            //await _hubContext.Clients.All.ReceiveNotification(msg, url);
            //var receiverConnectId = _connections.GetConnections(receiverId);
            foreach (var receiver in groupMember)
            {
                if (receiver.UserId != Guid.Parse(senderId))
                {
                    foreach (var connectionId in _connections.GetConnections(receiver.UserId.ToString()))
                    {
                        await _hubContext.Clients.Client(connectionId).ReceiveNotification(jsonNotice);

                    }
                    /// [OPTIMIZE] If create oke, i will a new create for group msg to move out create method of for loop
                    await _createNotifications.CreateNotitfication(senderId, receiver.UserId.ToString(), msgDB, url);
                }
            }


        }


        /// <summary>
        /// When the Receiver are connecting
        /// After Notification was created in DB, this method will be trigged and push list notifications to Receiver
        /// *Note: If Receiver disconnected or offline -> Use API 'GetNotificationsListByUserid' to get list Notifications when Receiver online
        /// </summary>
        /// <param name="context">This context transport from Notifications Hub</param>
        /// <param name="userId">ID of Receiver</param>
        /// <returns>
        /// 15 Newest Notificattions of Receiver
        /// </returns>
        public async Task PushAllNotifyByUserIdWithTableDependencyService(HubCallerContext context, string userId)
        {

            var receiverConnectId = _connections.GetConnections(userId);
            List<NotificationOutDTO> rawNotice = _getNotifications.GetNotifyByUserIds(userId);
            string jsonNotice = System.Text.Json.JsonSerializer.Serialize(rawNotice);

            foreach (var connectionId in receiverConnectId)
            {
                await _hubContext.Clients.Client(connectionId).listReceiveNotification(jsonNotice);
            }

        }

        public async Task SendEmailAsync(string toEmail, bool isActive, UserProfile userProfile, bool isCreate)
        {
            try
            {

                var mailMessage = new MailMessage();
                if (!isActive)
                {
                    string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\UserBlocked.html";
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();

                    mailMessage.From = new MailAddress("anhbqhe163864@fpt.edu.vn", "From FPT Social");
                    mailMessage.Subject = "[FUSP] Notifications User Management System";
                    //mailMessage.Body = $"Dear {userProfile.LastName},<br/><br/>Your account has been blocked due to policy violations.<br/><br/>Best regards,<br/>Support Team";
                    mailMessage.Body = MailText.Replace("[FullName]", userProfile.FirstName + userProfile.LastName);
                    mailMessage.IsBodyHtml = true;
                }
                else
                {
                    if (isCreate)
                    {
                        string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\UserCreated.html";
                        StreamReader str = new StreamReader(FilePath);
                        string MailText = str.ReadToEnd();
                        str.Close();

                        mailMessage.From = new MailAddress("anhbqhe163864@fpt.edu.vn", "From FPT Social");
                        mailMessage.Subject = "[FUSP] Notifications User Management System";
                        //mailMessage.Body = $"Dear {userProfile.LastName},<br/><br/>Your account has been created successfully. Hope you have a great experience!<br/><br/>Best regards,<br/>Support Team";
                        mailMessage.Body = MailText.Replace("[FullName]", userProfile.FirstName + userProfile.LastName);
                        mailMessage.IsBodyHtml = true;
                    }
                    else
                    {
                        string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\UserActived.html";
                        StreamReader str = new StreamReader(FilePath);
                        string MailText = str.ReadToEnd();
                        str.Close();

                        mailMessage.From = new MailAddress("anhbqhe163864@fpt.edu.vn", "From FPT Social");
                        mailMessage.Subject = "[FUSP] Notifications User Management System";
                        //mailMessage.Body = $"Dear {userProfile.LastName},<br/><br/>Your account has been updated profile successfully or activated by admin. Hope you have a great experience!<br/><br/>Best regards,<br/>Support Team";
                        mailMessage.Body = MailText.Replace("[FullName]", userProfile.FirstName + userProfile.LastName);
                        mailMessage.IsBodyHtml = true;
                    }


                }

                mailMessage.To.Add(toEmail);

                await _smtpClient.SendMailAsync(mailMessage);

            }
            catch
            {
                return;
            }
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(Period);
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                DateTime datetime = DateTime.Now;

            }
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
