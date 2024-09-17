using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReportType
{
    public class GetReportTypeQuery : IQuery<List<GetReportTypeResult>>
    {
    }
}
