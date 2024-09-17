using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetButtonFriend
{
    public class GetButtonFriendQuery : IQuery<GetButtonFriendQueryResult>
    {
        public Guid? UserId { get; set; }
        public Guid FriendId { get; set; }
    }
}
