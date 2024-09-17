using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByVideoPost
{
    public class GetReactByVideoPostQuery : IQuery<GetReactByVideoPostQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid UserPostVideoId { get; set; }
        public Guid UserId { get; set; }
    }
}
