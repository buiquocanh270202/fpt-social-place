using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CQRS.Query;

namespace Application.Queries.GetAllFriendRequested
{
    public class GetAllFriendRequestQuery : IQuery<List<GetAllFriendRequestQueryResult>>
    {
        public Guid? UserId { get; set; }
    }
}
