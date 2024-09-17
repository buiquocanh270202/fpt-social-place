using Application.DTO.GroupDTO;
using Application.Queries.FindUserByName;
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

namespace Application.Queries.GetGroupByName
{
    public class GetGroupByNameQueryHandler : IQueryHandler<GetGroupByNameQuery, GetGroupByNameQueryResult>
    {
        private readonly fptforumQueryContext _context;
        public GetGroupByNameQueryHandler(fptforumQueryContext context)
        {
            _context = context;
        }
        public async Task<Result<GetGroupByNameQueryResult>> Handle(GetGroupByNameQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new GetGroupByNameQueryResult();
            var normalizedSearchString = request.FindGroupName.RemoveDiacritics();
            var groups = await _context.GroupFpts.Include(x => x.GroupStatus)
                            .Include(x=>x.GroupType).Where(x => x.IsDelete == false).ToListAsync();
            var searchWords = normalizedSearchString.SplitIntoWords();

            var findGroup = groups.Select(group => new
                            {
                                Group = group,
                                NormalizedName = group.GroupName.RemoveDiacritics().ToLower(),
                                NameWords = group.GroupName.RemoveDiacritics().ToLower().SplitIntoWords(),
                                Permutations = group.GroupName.RemoveDiacritics().ToLower().SplitIntoWords().GetAllPermutations()
                            })
                            .Select(group => new
                            {
                                group.Group,
                                group.NormalizedName,
                                group.NameWords,
                                group.Permutations,
                                ExactMatch = group.NormalizedName.Equals(normalizedSearchString),
                                NoDiacriticsMatch = group.NormalizedName == normalizedSearchString,
                                ReverseNameMatch = string.Join(" ", group.NameWords.Reverse()) == normalizedSearchString,
                                ReverseNoDiacriticsMatch = string.Join(" ", group.NameWords.Reverse()) == normalizedSearchString,
                                PermutationMatch = group.Permutations.Contains(normalizedSearchString),
                                ContainsMostWords = searchWords.Count(word => group.NameWords.Contains(word)),
                                ContainsAnyWords = searchWords.All(word => group.NameWords.Contains(word)),
                                SubstringMatch = searchWords.Any(word => group.NormalizedName.Contains(word))
                            })
                            .OrderByDescending(u => u.ExactMatch)
                            .ThenByDescending(u => u.NoDiacriticsMatch)
                            .ThenByDescending(u => u.ReverseNameMatch)
                            .ThenByDescending(u => u.ReverseNoDiacriticsMatch)
                            .ThenByDescending(u => u.PermutationMatch)
                            .ThenByDescending(u => u.ContainsMostWords)
                            .ThenByDescending(u => u.ContainsAnyWords)
                            .ThenByDescending(u => u.SubstringMatch)
                            .ThenBy(u => u.NormalizedName)
                            .Select(u => u.Group)
                            .ToList();

            foreach (var group in findGroup)
            {
                var member = await _context.GroupMembers.Where(x => x.GroupId == group.GroupId && x.IsJoined == true).ToListAsync();
                var joined = await _context.GroupMembers.FirstOrDefaultAsync(x => x.GroupId == group.GroupId && x.UserId == request.UserId);
                var find = new GetGroupByNameDTO
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    CoverIgame = group.CoverImage,
                    GroupTypeId = group.GroupTypeId,
                    GroupType = group.GroupType.GroupTypeName,
                    MemberCount = member.Count
                };
                
                if(joined != null)
                {
                    result.GroupJoined.Add(find);
                }
                else
                {
                    result.GroupDontJoin.Add(find);
                }
            }

            return Result<GetGroupByNameQueryResult>.Success(result);
        }
    }
}
