using Application.DTO.FriendDTO;
using Application.Queries.GetAllFriend;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.SuggestFriend
{
    public class SuggestionFriendQueryHandler : IQueryHandler<SuggestionFriendQuery, SuggestionFriendQueryResult>
    {

        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public SuggestionFriendQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<SuggestionFriendQueryResult>> Handle(SuggestionFriendQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userFriends = await _context.Friends
                .Where(x => (x.UserId == request.UserId || x.FriendId == request.UserId) && x.Confirm == true)
                .Select(x => x.UserId == request.UserId ? x.FriendId : x.UserId)
                .ToListAsync();

            var result = new SuggestionFriendQueryResult();
            if (userFriends?.Count > 10)
            {
                var potentialFriends = new Dictionary<Guid, (int mutualFriends, int? reactCount)>();
                foreach (var friendId in userFriends)
                {
                    var friendsOfFriend = await _context.Friends
                        .Where(x => (x.UserId == friendId || x.FriendId == friendId) && x.Confirm == true)
                        .Select(x => x.UserId == friendId ? x.FriendId : x.UserId)
                        .ToListAsync();

                    foreach (var fofId in friendsOfFriend)
                    {
                        if (fofId != request.UserId && !userFriends.Contains(fofId))
                        {
                            if (potentialFriends.ContainsKey(fofId))
                            {
                                potentialFriends[fofId] = (potentialFriends[fofId].mutualFriends + 1, potentialFriends[fofId].reactCount);
                            }
                            else
                            {
                                var reactCount = await _context.Friends
                                    .Where(x => (x.UserId == fofId && x.FriendId == request.UserId) || (x.UserId == request.UserId && x.FriendId == fofId))
                                    .Select(x => x.ReactCount)
                                    .FirstOrDefaultAsync();

                                potentialFriends[fofId] = (1, reactCount);
                            }
                        }
                    }
                }

                var sortedPotentialFriends = potentialFriends
                    .OrderByDescending(x => (x.Value.mutualFriends + (x.Value.reactCount ?? 0)))
                    .Take(10)
                    .Select(x => x.Key)
                    .ToList();

              
                foreach (var friendId in sortedPotentialFriends)
                {
                    var friendProfile = await _context.UserProfiles
                        .Include(x=>x.AvataPhotos)
                        .Where(x => x.UserId == friendId)
                        .FirstOrDefaultAsync();
                    var requestFriend = await _context.Friends.FirstOrDefaultAsync(x => (x.UserId == friendId && x.FriendId == request.UserId)
                                                                                    || (x.UserId == request.UserId && x.FriendId == friendId));
                    if(requestFriend == null)
                    {
                        if (friendProfile != null)
                        {
                            var friendDTO = new GetAllFriendDTO
                            {
                                FriendId = friendProfile.UserId,
                                FriendName = $"{friendProfile.FirstName} {friendProfile.LastName}",
                                MutualFriends = potentialFriends[friendId].mutualFriends,
                                ReactCount = potentialFriends[friendId].reactCount,
                                Avata = friendProfile.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl
                                // Add other fields if needed
                            };
                            result.AllFriend.Add(friendDTO);
                        }
                    }
                    
                }
                result.Count = result.AllFriend.Count;
            }
            else
            {
                var current = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.UserId);
                var user = await _context.UserProfiles.Include(x=>x.AvataPhotos)
                    .Where(x => x.Campus == current.Campus && x.IsActive == true && x.UserId != current.UserId).ToListAsync();
                int i = 0;
                foreach (var item in user)
                {
                    var listSetting = await _context.UserSettings.Include(x => x.Setting)
                                        .Include(x => x.UserStatus).Where(x => x.UserId == item.UserId).ToListAsync();
                    var requestFriend = await _context.Friends.FirstOrDefaultAsync(x => (x.UserId == item.UserId && x.FriendId == request.UserId) 
                                                                                    || (x.UserId == request.UserId && x.FriendId == item.UserId));
                    if(requestFriend == null)
                    {
                        if(listSetting?.Count > 0)
                        {
                            if (listSetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Profile Status")).UserStatus.StatusName.Equals("Public")
                            && listSetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Send Friend Invitations")).UserStatus.StatusName.Equals("Public"))
                            {
                                var friendDTO = new GetAllFriendDTO
                                {
                                    FriendId = item.UserId,
                                    FriendName = $"{item.FirstName} {item.LastName}",
                                    Avata = item.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl,
                                    MutualFriends = 0,
                                    ReactCount = 0
                                };
                                result.AllFriend.Add(friendDTO);
                                i++;
                            }
                            if (i == 20)
                            {
                                break;
                            }
                        }
                    }
                }
                result.Count = result.AllFriend.Count;
            }
           

            return Result<SuggestionFriendQueryResult>.Success(result);
        }
    }
}
