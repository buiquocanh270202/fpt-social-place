using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByGroupPost
{
    public class GetReactByGroupPostQuery : IQuery<GetReactByGroupPostQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid GroupPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
