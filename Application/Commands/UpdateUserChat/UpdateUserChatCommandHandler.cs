using Application.Services;
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

namespace Application.Commands.UpdateUserChat
{
    public class UpdateUserChatCommandHandler : ICommandHandler<UpdateUserChatCommand, UpdateUserChatCommandResult>
    {
        private readonly fptforumQueryContext _querycontext;
        private readonly ChatEngineService _chatEngineService;


        public UpdateUserChatCommandHandler( fptforumQueryContext querycontext, ChatEngineService chatEngineService)
        {
            _chatEngineService = chatEngineService;
            _querycontext = querycontext;
        }
        public async Task<Result<UpdateUserChatCommandResult>> Handle(UpdateUserChatCommand request, CancellationToken cancellationToken)
        {
            if (_querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var chatuser = await _querycontext.ChatAccounts.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            if (chatuser == null)
            {
                throw new ErrorException(StatusCodeEnum.UC02_User_Chat_Is_Not_Exist);
            }
            var chat = await _chatEngineService.UpdateUserAsync(request.UserId.ToString(), chatuser.AccountId.ToString(), request.Email, request.FirstName, request.LastName, request.Avata);
            var result = new UpdateUserChatCommandResult
            {
                Message = "Update Success!"
            };

            return Result<UpdateUserChatCommandResult>.Success(result);
        }
    }
}
