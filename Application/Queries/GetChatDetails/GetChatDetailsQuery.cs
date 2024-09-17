using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetChatDetails
{
    public class GetChatDetailsQuery : IQuery<GetChatDetailsQueryResult>
    {
        public Guid? UserId { get; set; }
        public string ChatId { get; set; }
    }
}
