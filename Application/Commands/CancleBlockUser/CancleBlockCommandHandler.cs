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

namespace Application.Commands.CancleBlockUser
{
    public class CancleBlockCommandHandler : ICommandHandler<CancleBlockCommand, CancleBlockCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;

        public CancleBlockCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext)
        {
            _context = context;
            _querycontext = querycontext;
        }
        public async Task<Result<CancleBlockCommandResult>> Handle(CancleBlockCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var blockUser = await _querycontext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.BlockedUserId);
            var result = new CancleBlockCommandResult();
            if (blockUser == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }
            var getblock = await _querycontext.BlockUsers
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.UserIsBlockedId == request.BlockedUserId);

            var blocked = new Domain.CommandModels.BlockUser
            {
                BlockUserId = getblock.BlockUserId,
                UserId = getblock.UserId,
                UserIsBlockedId = getblock.UserIsBlockedId,
                BlockTypeId = getblock.BlockTypeId,
                CreatedAt = getblock.CreatedAt,
                IsBlock = getblock.IsBlock,
                UpdatedAt = getblock.UpdatedAt
            };

            _context.BlockUsers.Remove(blocked);
            await _context.SaveChangesAsync();
            result.Message = "Cancel Block Success!";
            result.IsCancle = true;
            return Result<CancleBlockCommandResult>.Success(result);
        }
    }
}
