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

namespace Application.Queries.GetAllRequestFriend
{
    public class GetAllRequestFriendQueryHandler : IQueryHandler<GetAllRequestFriendQuery, GetAllRequestFriendQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetAllRequestFriendQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetAllRequestFriendQueryResult>> Handle(GetAllRequestFriendQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var listfriend = await _context.Friends
                .Include(x => x.FriendNavigation)
                .Where(x => (x.UserId == request.UserId && x.Confirm == false)).ToListAsync();

            var listfriendrq = await _context.Friends
                .Include(x => x.User)
                .Where(x => (x.FriendId == request.UserId && x.Confirm == false)).ToListAsync();
            var list = new List<Domain.QueryModels.Friend>();
            list.AddRange(listfriend);
            list.AddRange(listfriendrq);
            var listallfriend = new List<Domain.QueryModels.UserProfile>();
            var listreact = new Dictionary<Guid, int?>();

            foreach (var fr in listfriendrq)
            {
                listreact.Add(fr.UserId, fr.ReactCount);
                var profile = await _context.UserProfiles.Include(x => x.AvataPhotos).FirstOrDefaultAsync(x => x.UserId == fr.UserId);
                listallfriend.Add(profile);
            }
            var result = new GetAllRequestFriendQueryResult();
            if (listallfriend != null)
            {
                result.Count = listallfriend.Count;
                foreach (var friend in listallfriend)
                {
                    var otherfriend = _context.Friends.Where(x => (x.UserId == friend.UserId && x.Confirm == true)
                                                        || (x.FriendId == friend.UserId && x.Confirm == true)).ToList();
                    var mutualfriend = otherfriend.Intersect(list);
                    var frienddto = new GetAllFriendDTO
                    {
                        FriendId = friend.UserId,
                        FriendName = friend.FirstName + " " + friend.LastName,
                        ReactCount = listreact[friend.UserId],
                        MutualFriends = mutualfriend.Count(),
                    };
                    if (friend.AvataPhotos.Count > 0)
                    {
                        frienddto.Avata = friend.AvataPhotos.FirstOrDefault(x => x.IsUsed == true).AvataPhotosUrl;
                    }
                    result.AllFriend.Add(frienddto);
                }

                result.AllFriend.OrderByDescending(x => x.ReactCount).OrderByDescending(x => x.MutualFriends);
            }
            else
            {
                result.Count = 0;
            }
            
            return Result<GetAllRequestFriendQueryResult>.Success(result);
        }
    }
}
