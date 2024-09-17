using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByGroupSharePostId
{
    public class GetReactByGroupSharePostQuery : IQuery<GetReactByGroupSharePostQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
    }
}
