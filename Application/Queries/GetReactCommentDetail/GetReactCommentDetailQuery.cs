using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactCommentDetail
{
    public class GetReactCommentDetailQuery : IQuery<GetReactCommentDetailQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid UserId { get; set; }
        public string? CommentType { get; set; }
        public Guid? CommentId { get; set; }
        public string? ReactName { get; set; }
    }
}
