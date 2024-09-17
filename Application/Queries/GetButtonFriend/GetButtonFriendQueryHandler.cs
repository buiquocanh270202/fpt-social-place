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

namespace Application.Queries.GetButtonFriend
{
    public class GetButtonFriendQueryHandler : IQueryHandler<GetButtonFriendQuery, GetButtonFriendQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;
        public GetButtonFriendQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetButtonFriendQueryResult>> Handle(GetButtonFriendQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var friend = await _context.Friends.FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.FriendId == request.FriendId)
                                                    || (x.FriendId == request.UserId && x.UserId == request.FriendId));
            var result = new GetButtonFriendQueryResult();
            if(friend != null)
            {
                if(friend.Confirm == true)
                {
                    result.Friend = true;
                }
                else
                {
                    if(request.UserId == friend.UserId)
                    {
                        result.Request = true;
                    }

                    if(request.UserId == friend.FriendId)
                    {
                        result.Confirm = true;
                    }
                }

            }
            else
            {
                result.Friend = false;
            }
            return Result<GetButtonFriendQueryResult>.Success(result);
        }
    }
}
