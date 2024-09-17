using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetAllFriendRequested
{
    public class GetAllFriendRequestQueryHandler : IQueryHandler<GetAllFriendRequestQuery, List<GetAllFriendRequestQueryResult>>
    {
        private readonly fptforumQueryContext _context;

        public GetAllFriendRequestQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
        }
        public async Task<Result<List<GetAllFriendRequestQueryResult>>> Handle(GetAllFriendRequestQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var requested = await _context.Friends.Include(x=>x.FriendNavigation)
                .ThenInclude(x=>x.AvataPhotos)
                .Where(x => x.UserId == request.UserId && x.Confirm == false).ToListAsync();
            var result = new List<GetAllFriendRequestQueryResult>();
            foreach (var item in requested)
            {
                var req = new GetAllFriendRequestQueryResult
                {
                    UserId = item.FriendId,
                    IsRequested = true,
                    UserName = item.FriendNavigation.FirstName + " "+ item.FriendNavigation.LastName,
                    Avata = item.FriendNavigation.AvataPhotos?.FirstOrDefault(x=>x.IsUsed).AvataPhotosUrl
                };
                result.Add(req);
            }
            return Result<List<GetAllFriendRequestQueryResult>>.Success(result);
        }
    }
}
