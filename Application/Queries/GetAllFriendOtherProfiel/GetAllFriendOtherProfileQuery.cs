using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendOtherProfiel
{
    public class GetAllFriendOtherProfileQuery : IQuery<GetAllFriendOtherProfileQueryResult>
    {
        public Guid? UserId { get; set; }
        public Guid? ViewUserId { get; set; }
    }
}
