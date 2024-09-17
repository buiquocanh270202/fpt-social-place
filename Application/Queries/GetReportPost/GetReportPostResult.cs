using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReportPost
{
    public class GetReportPostResult
    {
        public List<GetReportPost>? result { get; set; }
        public int? totalPage { get; set; }
    }

    public class GetReportPost
    {
        public Guid? UserPostId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? SharePostId { get; set; }
        public Guid? GroupSharePostId { get; set; }
        public int NumberReporter { get; set; }
    }
}
