using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllRequestFriend
{
    public class GetAllRequestFriendQuery : IQuery<GetAllRequestFriendQueryResult>
    {
        public Guid UserId { get; set; }
    }
}
