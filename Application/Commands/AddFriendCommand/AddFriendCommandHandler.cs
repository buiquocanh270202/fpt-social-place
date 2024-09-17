using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
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

namespace Application.Commands.AddFriendCommand
{
    public class AddFriendCommandHandler : ICommandHandler<AddFriendCommand, AddFriendCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        public AddFriendCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<AddFriendCommandResult>> Handle(AddFriendCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            if (request.UserId == request.FriendId)
            {
                throw new ErrorException(StatusCodeEnum.FR01_Cannot_Send);
            }
            var userreceipt = await _querycontext.UserProfiles.Include(x=>x.BlockUserUsers)
                .Include(x=>x.BlockUserUserIsBlockeds).FirstOrDefaultAsync(x => x.UserId == request.FriendId);
            var friend = await _querycontext.Friends.FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.FriendId == request.FriendId) || (x.UserId == request.FriendId
                                                                                && x.FriendId == request.UserId));
            if(friend != null)
            {
                throw new ErrorException(StatusCodeEnum.FR01_Cannot_Send);
            }
            
            if (userreceipt == null)
            {
                throw new ErrorException(StatusCodeEnum.FR01_Cannot_Send);
            }
            foreach (var item in userreceipt.BlockUserUserIsBlockeds)
            {
                if (item.UserIsBlockedId == request.UserId
                    || item.UserIsBlockedId == request.FriendId
                    || item.UserId == request.FriendId
                    || item.UserId == request.UserId)
                {
                    throw new ErrorException(StatusCodeEnum.U01_Not_Found);
                }
            }

            var newfriend = new Domain.CommandModels.Friend { 
                UserId = (Guid)request.UserId,
                FriendId = (Guid)request.FriendId,
                Confirm = false,
                ReactCount = null,
                LastInteractionDate = null,
            };
            await _context.Friends.AddAsync(newfriend);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<AddFriendCommandResult>(newfriend);
            return Result<AddFriendCommandResult>.Success(result);
        }
    }
}
