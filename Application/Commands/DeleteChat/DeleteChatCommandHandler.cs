using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Core.CQRS;
using Core.CQRS.Command;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Application.Commands.DeleteChat
{
    public class DeleteChatCommandHandler : ICommandHandler<DeleteChatCommand, DeleteChatCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly ChatEngineService _chatEngineService;


        public DeleteChatCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, ChatEngineService chatEngineService)
        {
            _chatEngineService = chatEngineService;
            _context = context;
            _querycontext = querycontext;
        }
        public async Task<Result<DeleteChatCommandResult>> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new DeleteChatCommandResult();
            var oldchat = await _querycontext.UserChats.FirstOrDefaultAsync(x => x.UserChatId == request.ChatId);
            if (oldchat == null)
            {
                throw new ErrorException(StatusCodeEnum.CH02_Can_Not_Delete_Chat);
            }

            var chat = await _chatEngineService.DeleteChatAsync(request.UserId.ToString(), request.ChatId.ToString());
            var getchat = JObject.Parse(chat);
            string id = getchat["id"].ToString();
            if(chat != null)
            {
                var removechat = new Domain.CommandModels.UserChat
                {
                    UserChatId = oldchat.UserChatId,
                    UserId = oldchat.UserId,
                    CreateDate = oldchat.CreateDate,
                    ChatWithId = oldchat.ChatWithId,
                };
                _context.UserChats.Remove(removechat);
                await _context.SaveChangesAsync();
                result.ChatId = result.ChatId;
                result.Message = "Delete chat success!";
            }

            return Result<DeleteChatCommandResult>.Success(result);    
        }
    }
}
