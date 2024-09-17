using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupByUserId
{
    public class GetGroupByUserIdQuery : IQuery<GetGroupByUserIdQueryResult>
    {
        public Guid? UserId { get; set; }
    }
}
