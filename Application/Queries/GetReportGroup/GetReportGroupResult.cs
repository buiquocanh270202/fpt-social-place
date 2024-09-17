using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReportGroup
{
    public class GetReportGroupResult
    {
        public List<GetReportGroup>? result { get; set; }
        public int? totalPage { get; set; }
    }

    public class GetReportGroup
    {
        public Guid? GroupReportedId { get; set; }
        public string GroupName { get; set; }
        public string GroupCoverImage { get; set; }
        public int NumberReporter { get; set; }
    }
}

