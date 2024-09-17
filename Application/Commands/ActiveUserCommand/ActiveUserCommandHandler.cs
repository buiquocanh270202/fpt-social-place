using Application.Commands.DeactiveUserCommand;
using Core.CQRS;
using Core.CQRS.Command;
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

namespace Application.Commands.ActiveUserCommand
{
    public class ActiveUserCommandHandler : ICommandHandler<ActiveUserCommand, ActiveUserCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;

        public ActiveUserCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext)
        {
            _context = context;
            _querycontext = querycontext;
        }
        public async Task<Result<ActiveUserCommandResult>> Handle(ActiveUserCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null || request.UserId == Guid.Empty)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var user = await _querycontext.UserProfiles
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }

            if (user.IsActive == true)
            {
                var result = new ActiveUserCommandResult();
                result.Message = "User is active.";
                return Result<ActiveUserCommandResult>.Success(result);
            }

            // Active trong bảng UserProfile
            var commandUser = await _context.UserProfiles
                .Where(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (commandUser == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }

            commandUser.IsActive = true;
            await _context.SaveChangesAsync(cancellationToken);
            var results = new ActiveUserCommandResult();
            results.Message = "Sucess to active user.";
            return Result<ActiveUserCommandResult>.Success(results);
        }
    }
}
