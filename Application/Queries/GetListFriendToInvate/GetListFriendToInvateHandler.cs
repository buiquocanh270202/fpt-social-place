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

namespace Application.Queries.GetListFriendToInvate
{
    public class GetListFriendToInvateHandler : IQueryHandler<GetListFriendToInvateQuery, List<GetListFriendToInvateResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetListFriendToInvateHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetListFriendToInvateResult>>> Handle(GetListFriendToInvateQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new List<GetListFriendToInvateResult>();
            var listfriend = await _context.Friends.Include(x => x.FriendNavigation)
                .Where(x => x.UserId == request.UserId && x.Confirm == true).ToListAsync();

            var listfriendrq = await _context.Friends
                .Include(x => x.User)
                .Where(x => (x.FriendId == request.UserId && x.Confirm == true)).ToListAsync();
            var list = new List<Domain.QueryModels.Friend>();
            list.AddRange(listfriendrq);
            var listallfriend = new List<Domain.QueryModels.UserProfile>();
            foreach (var fr in listfriend)
            {
                var profile = await _context.UserProfiles.Include(x => x.AvataPhotos).FirstOrDefaultAsync(x => x.UserId == fr.FriendId);
                listallfriend.Add(profile);
            }

            foreach (var fr in listfriendrq)
            {
                var profile = await _context.UserProfiles.Include(x => x.AvataPhotos).FirstOrDefaultAsync(x => x.UserId == fr.UserId);
                listallfriend.Add(profile);
            }

            if(listallfriend != null)
            {
                
                foreach (var item in listallfriend)
                {
                    var joined = await _context.GroupMembers.FirstOrDefaultAsync(x => x.GroupId == request.GroupId && x.UserId == item.UserId);
                    if(joined == null)
                    {
                        var friend = new GetListFriendToInvateResult
                        {
                            UserId = item.UserId,
                            Avata = item.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl,
                            UserName = item.FirstName + " " + item.LastName
                        };
                        result.Add(friend);
                    } 
                }

            }
            return Result<List<GetListFriendToInvateResult>>.Success(result);
        }
    }
}
