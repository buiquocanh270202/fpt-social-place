using Application.DTO.FriendDTO;
using Application.Queries.GetAllFriend;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendOtherProfiel
{
    public class GetAllFriendOtherProfileQueryHandler : IQueryHandler<GetAllFriendOtherProfileQuery, GetAllFriendOtherProfileQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetAllFriendOtherProfileQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetAllFriendOtherProfileQueryResult>> Handle(GetAllFriendOtherProfileQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var getusersetting = await _context.UserSettings.Include(x => x.Setting).Where(x => x.UserId == request.ViewUserId).ToListAsync();
            var getuser = await _context.UserProfiles
                                .Include(x => x.UserSettings)
                                .FirstOrDefaultAsync(x => x.UserId == request.ViewUserId);
            var listmyfriend = await _context.Friends
                .Include(x => x.FriendNavigation)
                .Where(x => (x.UserId == request.UserId && x.Confirm == true) 
                ||(x.FriendId == request.UserId && x.Confirm == true)).ToListAsync();
            var getstatus = await _context.UserStatuses.ToListAsync();
            var listfriend = await _context.Friends
              .Include(x => x.FriendNavigation)
              .Where(x => (x.UserId == request.ViewUserId && x.Confirm == true)).ToListAsync();

            var listfriendrq = await _context.Friends
                .Include(x => x.User)
                .Where(x => (x.FriendId == request.ViewUserId && x.Confirm == true)).ToListAsync();
            var list = new List<Domain.QueryModels.Friend>();
            list.AddRange(listfriend);
            list.AddRange(listfriendrq);
            var listallfriend = new List<Domain.QueryModels.UserProfile>();
            var listreact = new Dictionary<Guid, int?>();
            foreach (var fr in listfriend)
            {
                listreact.Add(fr.FriendId, fr.ReactCount);
                var profile = await _context.UserProfiles.Include(x => x.AvataPhotos).FirstOrDefaultAsync(x => x.UserId == fr.FriendId);
                listallfriend.Add(profile);
            }

            foreach (var fr in listfriendrq)
            {
                listreact.Add(fr.UserId, fr.ReactCount);
                var profile = await _context.UserProfiles.Include(x => x.AvataPhotos).FirstOrDefaultAsync(x => x.UserId == fr.UserId);
                listallfriend.Add(profile);
            }
            var result = new GetAllFriendOtherProfileQueryResult();

            if (getusersetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Profile Status")).UserStatusId
               == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId)
            {
                result = null;
                return Result<GetAllFriendOtherProfileQueryResult>.Success(result);
            }

            if (getusersetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Profile Status")).UserStatusId
               == getstatus.FirstOrDefault(x => x.StatusName == "Friend").UserStatusId)
            {
                var isfriend = _context.Friends.FirstOrDefault(x => (x.FriendId == request.UserId && x.Confirm == true) || (x.UserId == request.UserId && x.Confirm == true));
                if(isfriend == null)
                {
                    result = null;
                    return Result<GetAllFriendOtherProfileQueryResult>.Success(result);
                }  
            }

            if (listallfriend != null)
            {
                result.Count = listfriend.Count;
                foreach (var friend in listallfriend)
                {
                   if(friend.IsActive == true)
                    {
                        var otherfriend = _context.Friends
                            .Where(x => (x.UserId == friend.UserId && x.Confirm == true)
                            ||(x.FriendId == friend.UserId && x.Confirm == true)).ToList();
                        var mutualfriend = otherfriend.Intersect(listmyfriend);
                        var frienddto = new GetAllFriendDTO
                        {
                            FriendId = friend.UserId,
                            FriendName = friend.FirstName + " " + friend.LastName,
                            ReactCount = listreact[friend.UserId],
                            MutualFriends = mutualfriend.Count()
                        };
                        if(friend.UserId == request.UserId)
                        {
                            frienddto.MutualFriends = 0;
                        }
                        if (friend.AvataPhotos.Count > 0)
                        {
                            frienddto.Avata = friend.AvataPhotos.FirstOrDefault(x => x.IsUsed == true).AvataPhotosUrl;
                        }
                        result.AllFriend.Add(frienddto);
                    }
                }
                result.AllFriend.OrderByDescending(x => x.ReactCount).OrderByDescending(x => x.MutualFriends);
                result.Count = result.AllFriend.Count;
            }
            else
            {
                result.Count = 0;
            }

            return Result<GetAllFriendOtherProfileQueryResult>.Success(result);
        }
    }
}
