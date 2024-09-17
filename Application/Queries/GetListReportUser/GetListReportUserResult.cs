using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListReportUser
{
    public class GetListReportUserResult
    {
        public List<GetReportUser>? result {  get; set; }
        public int? totalPage { get; set; }
    }

    public class GetReportUser
    {
        public Guid ReportId { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid ReportedUserId { get; set; }
        public string? ReportedUserName { get; set; }
        public string? ReportedAvatarUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
