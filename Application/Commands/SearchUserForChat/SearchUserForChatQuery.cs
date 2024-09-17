using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.SearchUserForChat
{
    public class SearchUserForChatQuery : IQuery<SearchUserForChatQueryResult>
    {
        public Guid? UserId { get; set; }
        public string SearchName { get; set; }
    }
}
