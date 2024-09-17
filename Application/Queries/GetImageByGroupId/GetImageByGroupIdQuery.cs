using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetImageByGroupId
{
    public class GetImageByGroupIdQuery : IQuery<GetImageByGroupIdQueryResult>
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public int Page { get; set; } = 1;
    }
}
