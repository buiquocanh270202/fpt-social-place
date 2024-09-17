using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReportUser
{
    public class GetReportUserResult
    {
        public List<GetReportUser>? result { get; set; }
        public int? totalPage { get; set; }
    }

    public class GetReportUser
    {
        public Guid? UserReportedId { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public int NumberReporter { get; set; }
    }
}
