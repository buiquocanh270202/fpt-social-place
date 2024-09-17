using Application.Services;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserOnChat
{
    public class GetUserOnChatQueryHandler : IQueryHandler<GetUserOnChatQuery, GetUserOnChatQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly ChatEngineService _services;

        public GetUserOnChatQueryHandler(fptforumQueryContext context, ChatEngineService service)
        {
            _context = context;
            _services = service;
        }
        public async Task<Result<GetUserOnChatQueryResult>> Handle(GetUserOnChatQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var getuserchat = await _services.SearchUserByName();
            var result = new GetUserOnChatQueryResult();
            var users = JsonConvert.DeserializeObject<List<User>>(getuserchat);
            foreach (var item in users)
            {
                var userId = Guid.Parse(item.Username);
                var user = await _context.UserProfiles.Include(x => x.AvataPhotos).FirstOrDefaultAsync(x => x.UserId == userId);
                if(user == null)
                {
                    continue;
                }
                var blockuser = await _context.BlockUsers
                    .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.UserIsBlockedId == userId
                                        || x.UserIsBlockedId == request.UserId && x.UserId == userId);
                var friend = await _context.Friends.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.FriendId == userId
                                        || x.FriendId == request.UserId && x.UserId == userId);
                var getusersetting = await _context.UserSettings.Include(x => x.Setting)
                    .Where(x => x.UserId == userId).ToListAsync();
                var getpublicsetting = await _context.UserStatuses.FirstOrDefaultAsync(x => x.StatusName == "Public");
                if(user != null)
                {
                    if (user?.AvataPhotos?.Count > 0)
                    {
                        item.Avata = user.AvataPhotos.FirstOrDefault(x => x.IsUsed).AvataPhotosUrl;
                    }
                    item.FullName = user.FirstName + " " + user.LastName;
                }
                if (blockuser == null)
                {
                    if (friend != null)
                    {
                        if(friend.Confirm == true)
                        {
                            result.ListFriend.Add(item);
                        }
                        else
                        {
                            if(getusersetting.FirstOrDefault(x=>x.Setting.SettingName.Equals("Profile Status")).UserStatusId
                                == getpublicsetting.UserStatusId)
                            {
                                result.OtherUser.Add(item);
                            }
                        }
                        
                    }
                    else
                    {
                        if (getusersetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Profile Status")).UserStatusId
                               == getpublicsetting.UserStatusId)
                        {
                            result.OtherUser.Add(item);
                        }
                    }
                }
            }

            return Result<GetUserOnChatQueryResult>.Success(result);

        }
    }
}
