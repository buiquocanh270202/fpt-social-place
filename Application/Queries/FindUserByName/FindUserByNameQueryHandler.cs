using Application.DTO.FriendDTO;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Core.Helper;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.FindUserByName
{
    public class FindUserByNameQueryHandler : IQueryHandler<FindUserByNameQuery, FindUserByNameQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;
        public FindUserByNameQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<FindUserByNameQueryResult>> Handle(FindUserByNameQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new FindUserByNameQueryResult();
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
                var newFind = new GetAllFriendDTO
                {
                    FriendId = user.UserId,
                    FriendName = user.FullName,
                    MutualFriends = 0,
                    ReactCount = 0,
                    Avata = user.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl
                };
                var friend = await _context.Friends.FirstOrDefaultAsync(x =>
                    ((x.UserId == request.UserId && x.FriendId == user.UserId) ||
                    (x.UserId == user.UserId && x.FriendId == request.UserId)) && x.Confirm == true);
                if(friend != null)
                {
                    result.ListFriend.Add(newFind);
                }
                else
                {
                    result.ListUserNotFriend.Add(newFind);   
                }

            }
            return Result<FindUserByNameQueryResult>.Success(result);

        }
    }
}
