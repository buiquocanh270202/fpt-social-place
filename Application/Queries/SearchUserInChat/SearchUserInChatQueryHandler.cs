using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FriendDTO;
using Application.Queries.FindUserByName;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Core.Helper;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.SearchUserInChat
{
    public class SearchUserInChatQueryHandler : IQueryHandler<SearchUserInChatQuery, SearchUserInChatQueryResult>
    {
        private readonly fptforumQueryContext _context;
        public SearchUserInChatQueryHandler(fptforumQueryContext context)
        {
            _context = context;
        }
        public async Task<Result<SearchUserInChatQueryResult>> Handle(SearchUserInChatQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new SearchUserInChatQueryResult();
            var normalizedSearchString = request.FindName.ToLower().RemoveDiacritics();
            var searchWords = normalizedSearchString.SplitIntoWords();
            var users = await _context.UserProfiles.Include(x => x.AvataPhotos).Where(x => x.IsActive == true).ToListAsync();
            var listFind = users
                            .Select(user => new
                            {
                                User = user,
                                NormalizedName = user.FullName.RemoveDiacritics().ToLower(),
                                NameWords = user.FullName.RemoveDiacritics().ToLower().SplitIntoWords(),
                                Permutations = user.FullName.RemoveDiacritics().ToLower().SplitIntoWords().GetAllPermutations()
                            })
                            .Select(user => new
                            {
                                user.User,
                                user.NormalizedName,
                                user.NameWords,
                                user.Permutations,
                                ExactMatch = user.NormalizedName.Equals(normalizedSearchString),
                                NoDiacriticsMatch = user.NormalizedName == normalizedSearchString,
                                ReverseNameMatch = string.Join(" ", user.NameWords.Reverse()) == normalizedSearchString,
                                ReverseNoDiacriticsMatch = string.Join(" ", user.NameWords.Reverse()) == normalizedSearchString,
                                PermutationMatch = user.Permutations.Contains(normalizedSearchString),
                                ContainsMostWords = searchWords.Count(word => user.NameWords.Contains(word)),
                                ContainsAnyWords = searchWords.All(word => user.NameWords.Contains(word)),
                                SubstringMatch = searchWords.Any(word => user.NormalizedName.Contains(word))
                            })
                            .Where(u => u.ExactMatch || u.NoDiacriticsMatch || u.ReverseNameMatch ||
                                        u.ReverseNoDiacriticsMatch || u.PermutationMatch ||
                                        u.ContainsMostWords > 0 || u.ContainsAnyWords || u.SubstringMatch)
                            .OrderByDescending(u => u.ExactMatch)
                            .ThenByDescending(u => u.NoDiacriticsMatch)
                            .ThenByDescending(u => u.ReverseNameMatch)
                            .ThenByDescending(u => u.ReverseNoDiacriticsMatch)
                            .ThenByDescending(u => u.PermutationMatch)
                            .ThenByDescending(u => u.ContainsMostWords)
                            .ThenByDescending(u => u.ContainsAnyWords)
                            .ThenByDescending(u => u.SubstringMatch)
                            .ThenBy(u => u.NormalizedName)
                            .Select(u => u.User)
                            .ToList();

            foreach (var user in listFind)
            {
                var block = await _context.BlockUsers
                    .FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.UserIsBlockedId == user.UserId)
                                            || (x.UserIsBlockedId == request.UserId && x.UserId == user.UserId));
                
                if(block != null)
                {
                    continue;
                }

                var oldchat = await _context.UserChats
                    .FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.ChatWithId == user.UserId)
                                            || (x.ChatWithId == request.UserId && x.UserId == user.UserId));
                var newFind = new GetUserChatDTO
                {
                    FriendId = user.UserId,
                    FriendName = user.FullName,
                    MutualFriends = 0,
                    ReactCount = 0,
                    Avata = user.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl,
                    
                };
                if(oldchat != null)
                {
                    newFind.ChatId = oldchat.UserChatId;
                }
                else
                {
                    newFind.ChatId = 0;
                }
                var friend = await _context.Friends.FirstOrDefaultAsync(x =>
                    ((x.UserId == request.UserId && x.FriendId == user.UserId) ||
                    (x.UserId == user.UserId && x.FriendId == request.UserId)) && x.Confirm == true);
                if (friend != null)
                {
                    result.ListFriend.Add(newFind);
                }
                else
                {
                    var usersetting = await _context.UserSettings.Include(x => x.Setting).Where(x => x.UserId == user.UserId).ToListAsync();
                    var publicsetting = await _context.UserStatuses.FirstOrDefaultAsync(x => x.StatusName.Equals("Public"));
                    if(usersetting.FirstOrDefault(x=>x.Setting.SettingName.Equals("Profile Status"))?.UserStatusId == publicsetting?.UserStatusId)
                    {
                        result.ListUserNotFriend.Add(newFind);
                    }else
                    {
                        if(oldchat != null)
                        {
                            result.ListUserNotFriend.Add(newFind);
                        }
                    }
                }

            }
            return Result<SearchUserInChatQueryResult>.Success(result);

        }
    }
}
