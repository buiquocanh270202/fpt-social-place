using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetVideoByGroupId
{
    public class GetVideoByGroupIdQuery : IQuery<GetVideoByGroupIdQueryResult>
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public int Page { get; set; }
    }
}
