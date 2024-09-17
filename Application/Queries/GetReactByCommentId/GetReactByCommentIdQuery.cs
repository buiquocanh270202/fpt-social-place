using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByCommentId
{
    public class GetReactByCommentIdQuery : IQuery<GetReactByCommentIdQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
    }
}
