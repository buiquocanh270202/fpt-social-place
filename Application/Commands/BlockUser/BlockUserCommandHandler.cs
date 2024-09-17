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

namespace Application.Commands.BlockUser
{
    public class BlockUserCommandHandler : ICommandHandler<BlockUserCommand, BlockUserCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly GuidHelper _helper;

        public BlockUserCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext)
        {
            _context = context;
            _querycontext = querycontext;
            _helper = new GuidHelper();
        }
        public async Task<Result<BlockUserCommandResult>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new BlockUserCommandResult();
            var blockUser = await _querycontext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.BlockUserId);
            var blockType = await _querycontext.BlockTypes.FirstOrDefaultAsync(x => x.BlockTypeName.Equals("FullBlock"));
            var friend = await _querycontext.Friends
                .FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.FriendId == request.BlockUserId)
                || (x.UserId == request.BlockUserId && x.FriendId == request.UserId));
            
            if(blockUser == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }

            var blocked = new Domain.CommandModels.BlockUser
            {
                BlockUserId = _helper.GenerateNewGuid(),
                UserId = (Guid)request.UserId,
                UserIsBlockedId = request.BlockUserId,
                BlockTypeId = blockType.BlockTypeId,
                CreatedAt = DateTime.Now,
                IsBlock = true,
                UpdatedAt = null
            };

            if(friend != null)
            {
                var friendBlock = new Domain.CommandModels.Friend
                {
                    UserId = friend.UserId,
                    FriendId = friend.FriendId,
                    Confirm = friend.Confirm,
                    ReactCount = friend.ReactCount,
                    LastInteractionDate = friend.LastInteractionDate
                };
                _context.Friends.Remove(friendBlock);
            }
            await _context.BlockUsers.AddAsync(blocked);
            await _context.SaveChangesAsync();
            result.Message = "Block Success!";
            result.IsBlocked = true;
            return Result<BlockUserCommandResult>.Success(result);
        }
    }
}
