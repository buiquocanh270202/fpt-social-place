using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactBySharePostId
{
    public class GetReactBySharePostQuery : IQuery<GetReactBySharePostQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid SharePostId { get; set; }
        public Guid UserId { get; set; }
    }
}
