using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllGroupStatus
{
    public class GetGroupStatusQuery : IQuery<List<GetGroupStatusQueryResult>>
    {
    }
}
