using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReportPost
{
    public class GetReportPostQuery : IQuery<GetReportPostResult>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
