using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetOtherUserPostByUserId
{
    public class GetOtherUserPostByUserIdQuery : IQuery<GetOtherUserPostByUserIdResult>
    {
        public Guid? UserId { get; set; }
        public Guid OtherUserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
