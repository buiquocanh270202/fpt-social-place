using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Hub;
using Application.Commands.CreateNotifications;
using Application.Hub;
using Application.Queries.GetNotifications;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet.Actions;
using MediatR;


namespace Application.Hub
{
    public enum NotificationsTypeEnum
    {
        NORMAL,
        IMPORTANCE
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsHub : Hub<INotificationsClient>
    {
        private readonly ConnectionMapping<string> _connections;
        private readonly IConfiguration _configuration;
        public readonly ICreateNotifications _createNotifications;
        public readonly INotificationsHubBackgroundService _INotificationsHubBackgroundService;

        public NotificationsHub(IConfiguration configuration, INotificationsHubBackgroundService notificationsHubBackgroundService, ICreateNotifications createNotifications, ConnectionMapping<string> connections)
        {
            _configuration = configuration;
            _INotificationsHubBackgroundService = notificationsHubBackgroundService;
            _createNotifications = createNotifications;
            _connections = connections;

        }

        public override async Task OnConnectedAsync()
        {
            HttpContext _httpContext = Context.GetHttpContext();
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
                await Clients.Client(Context.ConnectionId).ReceiveNotification($"The  ({Context.ConnectionId}) Connected Fail!");
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                await Clients.Client(Context.ConnectionId).ReceiveNotification($"The  ({Context.ConnectionId}) Connected Fail!");
            }
            var userId = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;
            _connections.Add(userId, Context.ConnectionId);
            //await Clients.Client(Context.ConnectionId).ReceiveNotification($"The {Context.User?.Identity?.Name} ({Context.ConnectionId}) connected success!");
            await _INotificationsHubBackgroundService.PushAllNotifyByUserIdWithTableDependencyService(Context, userId);
            await Clients.Client(Context.ConnectionId).ReceiveNotification($"The  ({Context.ConnectionId}) connected success!");


        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            HttpContext _httpContext = Context.GetHttpContext();
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
                await Clients.Client(Context.ConnectionId).ReceiveNotification($"The  ({Context.ConnectionId}) Connected Fail!");
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                await Clients.Client(Context.ConnectionId).ReceiveNotification($"The  ({Context.ConnectionId}) Connected Fail!");
            }
            var userId = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;
            _connections.Remove(userId, Context.ConnectionId);
        }

        public async Task OnReconnected()
        {
            HttpContext _httpContext = Context.GetHttpContext();
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
                await Clients.Client(Context.ConnectionId).ReceiveNotification($"The  ({Context.ConnectionId}) Connected Fail!");
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                await Clients.Client(Context.ConnectionId).ReceiveNotification($"The  ({Context.ConnectionId}) Connected Fail!");
            }
            var userId = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;

            if (!_connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                _connections.Add(userId, Context.ConnectionId);
            }

        }

        //===================================================================================== Notify For Admin/Staff's Actions ===================================================================================== 

        public async Task PushNotifyToAllUsers(string sender, string content, string url)
        {

            string msg = sender + _configuration["MessageContents:Common-004"] + content;
            await Clients.All.ReceiveNotification(msg, url);

        }

        public async Task PushNotifyToListUsers(string sender, string[] receiver, string content, string url)
        {
            string msg = sender + _configuration["MessageContents:Common-004"] + content;
            await Clients.Users(receiver).ReceiveNotification(msg, url);

        }

        public async Task PushNotifyToUser(string sender, string receiver, string content, string url)
        {
            string msg = sender + _configuration["MessageContents:Common-004"] + content;
            await Clients.User(receiver).ReceiveNotification(msg, url);

        }
        //===================================================================================== Notify For Member's Actions ===================================================================================== 

        public async Task SendNotify(string notice)
        {

            await _INotificationsHubBackgroundService.SendNotifyService(Context ,notice);

        }

        public async Task SendNotifyByGroupId(string notice)
        {

            await _INotificationsHubBackgroundService.SendGroupNotifyService(Context, notice);

        }

        public async Task PushAllNotifyByUserIdWithTableDependency(string userId)
        {

            await _INotificationsHubBackgroundService.PushAllNotifyByUserIdWithTableDependencyService(Context, userId);

        }

    }

}
