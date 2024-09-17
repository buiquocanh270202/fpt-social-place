using AutoMapper;
using Core.CQRS.Query;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserOnChat
{
    public class GetUserOnChatQuery : IQuery<GetUserOnChatQueryResult>
    {
        public Guid? UserId { get; set; }
    }
}
