using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupByGroupId
{
    public class GetGroupByGroupIdQuery : IQuery<GetGroupByGroupIdQueryResult>
    {
        public Guid? GroupId { get; set; }
        public Guid? UserId { get; set; }
    }
}
