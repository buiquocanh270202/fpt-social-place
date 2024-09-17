using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CQRS.Query;

namespace Application.Queries.SearchUserInChat
{
    public class SearchUserInChatQuery : IQuery<SearchUserInChatQueryResult>
    {
        public Guid? UserId { get; set; }
        public string FindName { get; set; }
    }
}
