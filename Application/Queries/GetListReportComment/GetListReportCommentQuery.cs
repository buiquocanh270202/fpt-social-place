using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListReportComment
{
    public class GetListReportCommentQuery : IQuery<GetListReportCommentResult>
    {
        public Guid? CommentId { get; set; }
        public Guid? CommentPhotoPostId { get; set; }
        public Guid? CommentVideoPostId { get; set; }
        public Guid? CommentGroupPostId { get; set; }
        public Guid? CommentPhotoGroupPostId { get; set; }
        public Guid? CommentGroupVideoPostId { get; set; }
        public Guid? CommentSharePostId { get; set; }
        public Guid? CommentGroupSharePostId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
