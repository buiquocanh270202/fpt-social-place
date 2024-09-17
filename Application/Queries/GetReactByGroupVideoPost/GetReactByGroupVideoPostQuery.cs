using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByGroupVideoPost
{
    public class GetReactByGroupVideoPostQuery : IQuery<GetReactByGroupVideoPostQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid GroupPostVideoId { get; set; }
        public Guid UserId { get; set; }
    }
}
