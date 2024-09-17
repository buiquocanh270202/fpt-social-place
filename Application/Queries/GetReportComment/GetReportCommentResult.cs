using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReportComment
{
    public class GetReportCommentResult
    {
        public List<GetReportComment>? result { get; set; }
        public int? totalPage { get; set; }
    }

    public class GetReportComment
    {
        public Guid? CommentId { get; set; }
        public Guid? CommentPhotoPostId { get; set; }
        public Guid? CommentVideoPostId { get; set; }
        public Guid? CommentGroupPostId { get; set; }
        public Guid? CommentPhotoGroupPostId { get; set; }
        public Guid? CommentGroupVideoPostId { get; set; }
        public Guid? CommentSharePostId { get; set; }
        public Guid? CommentGroupSharePostId { get; set; }
        public string? Content { get; set; }
        public int? NumberReporter { get; set; }
    }
}
