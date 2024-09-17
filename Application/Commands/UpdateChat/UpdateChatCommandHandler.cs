using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Commands.DeleteChat;
using Application.Services;
using Core.CQRS;
using Core.CQRS.Command;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Application.Commands.UpdateChat
{
    public class UpdateChatCommandHandler : ICommandHandler<UpdateChatCommand, UpdateChatCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly ChatEngineService _chatEngineService;


        public UpdateChatCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, ChatEngineService chatEngineService)
        {
            _chatEngineService = chatEngineService;
            _context = context;
            _querycontext = querycontext;
        }
        public async Task<Result<UpdateChatCommandResult>> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new UpdateChatCommandResult();
            var oldchat = await _querycontext.UserChats.FirstOrDefaultAsync(x => x.UserChatId == request.ChatId);
            if (oldchat == null)
            {
                throw new ErrorException(StatusCodeEnum.CH03_Can_Not_Update_Chat);
            }

            var chat = await _chatEngineService.UpdateChatAsync(request.UserId.ToString(), request.ChatId.ToString(), request.ChatName);
            var getchat = JObject.Parse(chat);
            string id = getchat["id"].ToString();
            if (chat != null)
            {
                result.ChatId = result.ChatId;
                result.ChatName = request.ChatName;
                result.Message = "Update chat success!";
            }
            return Result<UpdateChatCommandResult>.Success(result);
        }
    }
}
