using Application.Services;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateUserChat
{
    public class CreateUserChatCommandHandler : ICommandHandler<CreateUserChatCommand, CreateUserChatCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly ChatEngineService _chatEngineService;
        

        public CreateUserChatCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, ChatEngineService chatEngineService)
        {
            _chatEngineService = chatEngineService;
            _context = context;
            _querycontext = querycontext;
        }
        public async Task<Result<CreateUserChatCommandResult>> Handle(CreateUserChatCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            
            var chat = await _chatEngineService.CreateUserAsync(request.UserId.ToString(), request.Email, request.FirstName, request.LastName,request.Avata);
            var getchat = JObject.Parse(chat);
            string id = getchat["id"].ToString();
            var chatuser = await _querycontext.ChatAccounts.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.AccountId == id);
            if(chatuser!= null)
            {
                throw new ErrorException(StatusCodeEnum.UC01_User_Chat_Is_Exist);
            }
            var newaccount = new Domain.CommandModels.ChatAccount
            {
                AccountId = id,
                UserId = (Guid)request.UserId,
                CreateAt = DateTime.Now,
            };
            await _context.ChatAccounts.AddAsync(newaccount);
            await _context.SaveChangesAsync();
            var result = new CreateUserChatCommandResult
            {
                UserName = request.UserId.ToString(),
                Messsage = "Create User Success!"
            };

            return Result<CreateUserChatCommandResult>.Success(result);
        }
    }
}
