using Application.Commands.CreateUserChat;
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
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateChatBox
{
    public class CreateChatBoxCommandHandler : ICommandHandler<CreateChatBoxCommand, CreateChatBoxCommandResult>
    {
        private readonly ChatEngineService _service;
        private readonly GuidHelper _helper;
        private readonly fptforumQueryContext _context;
        private readonly fptforumCommandContext _commandContext;

        public CreateChatBoxCommandHandler(IMapper mapper, ChatEngineService service, fptforumQueryContext context, fptforumCommandContext commandContext)
        {
            _helper = new GuidHelper();
            _service = service;
            _context = context;
            _commandContext = commandContext;
        }
        public async Task<Result<CreateChatBoxCommandResult>> Handle(CreateChatBoxCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new CreateChatBoxCommandResult();
            var otherChat = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.OtherId);
            var block = await _context.BlockUsers.FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.UserIsBlockedId == request.OtherId)
                                                  || (x.UserId == request.OtherId && x.UserIsBlockedId == request.UserId));
            string idchat = "";
            if(block != null || otherChat.IsActive == false)
            {
                throw new ErrorException(StatusCodeEnum.CH01_Can_Not_Chat);
            }
            var oldChat = await _context.UserChats
                .FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.ChatWithId == request.OtherId)
                                        || (x.ChatWithId == request.UserId && x.UserId == request.OtherId));
            if(oldChat == null)
            {
                if (string.IsNullOrEmpty(request.Title))
                {
                    request.Title = "";
                }
                request.Title = otherChat.FirstName + " " + otherChat.LastName;
                var chat = await _service.CreateChatAsync(request.Title, request.UserId.ToString());
                var getchat = JObject.Parse(chat);
                string id = getchat["id"].ToString();
                idchat = id;
                var addmember = await _service.AddMemberToChatAsync(request.OtherId.ToString(), id, request.UserId.ToString());
                var newchat = new Domain.CommandModels.UserChat
                {
                    UserChatId = int.Parse(id),
                    UserId = (Guid)request.UserId,
                    CreateDate = DateTime.Now,
                    ChatWithId = request.OtherId,
                };
                await _commandContext.UserChats.AddAsync(newchat);
                await _commandContext.SaveChangesAsync();
                result.Title = request.Title;
                result.Message = "Create Chat Box Success!";
                result.ChatId = idchat;
            }
            else
            {
                result.Title = "";
                result.Message = "Can Not Create Chat Box";
                result.ChatId = "";
            }
            
            return Result<CreateChatBoxCommandResult>.Success(result);
        }
    }
}
