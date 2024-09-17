using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupStatusForCreate
{
    public class GetGroupStatusForCreateQuery :IQuery<List<GetGroupStatusForCreateQueryResult>>
    {
    }
}
