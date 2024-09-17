using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListReportPost
{
    public class GetListReportPostQuery : IQuery<GetListReportPostResult>
    {
        public Guid? UserPostId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? SharePostId { get; set; }
        public Guid? GroupSharePostId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
