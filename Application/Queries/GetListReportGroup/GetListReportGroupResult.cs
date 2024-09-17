using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListReportGroup
{
    public class GetListReportGroupResult
    {
        public List<GetReportGroup>? result { get; set; }
        public int? totalPage { get; set; }
    }

    public class GetReportGroup
    {
        public Guid ReportId { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid ReportedGroupId { get; set; }
        public string? ReportedGroupName { get; set; }
        public string? ReportedGroupCoverImage { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
