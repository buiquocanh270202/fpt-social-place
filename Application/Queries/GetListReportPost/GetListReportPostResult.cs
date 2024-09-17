using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListReportPost
{
    public class GetListReportPostResult
    {
        public List<GetReportPost>? result {  get; set; }
        public int? totalPage { get; set; }
    }

    public class GetReportPost
    {
        public Guid ReportPostId { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? SharePostId { get; set; }
        public Guid? GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? CreatedDate { get; set; }   
    }
}
