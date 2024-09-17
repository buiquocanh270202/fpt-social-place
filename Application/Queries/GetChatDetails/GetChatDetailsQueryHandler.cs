using Application.Services;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
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

namespace Application.Queries.GetChatDetails
{
    public class GetChatDetailsQueryHandler : IQueryHandler<GetChatDetailsQuery, GetChatDetailsQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;
        private readonly ChatEngineService _service;

        public GetChatDetailsQueryHandler(fptforumQueryContext context, ChatEngineService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<Result<GetChatDetailsQueryResult>> Handle(GetChatDetailsQuery request, CancellationToken cancellationToken)
        {

            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var listPersom = new List<Guid>();
            var result = new GetChatDetailsQueryResult();

            var chat = await _service
                .GetChatDetails(request.ChatId, request.UserId.ToString());
            var jsonObject = JObject.Parse(chat);
            result.Result = jsonObject;
            result.IsBloked = false;
            var adminUsername = Guid.Parse(jsonObject["admin"]["username"].ToString());

            JArray people = (JArray)jsonObject["people"];
            foreach (var personObj in people)
            {
                var personUsername = Guid.Parse(personObj["person"]["username"].ToString());
                listPersom.Add(personUsername);
            }
            if(listPersom.Count >= 1)
            {
                var block = await _context.BlockUsers.FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.UserIsBlockedId == listPersom[1])
                                                 || (x.UserId == listPersom[1] && x.UserIsBlockedId == request.UserId));
                if(block != null)
                {
                    result.IsBloked = true;
                }
            }
            return Result<GetChatDetailsQueryResult>.Success(result);
        }
    }
}
