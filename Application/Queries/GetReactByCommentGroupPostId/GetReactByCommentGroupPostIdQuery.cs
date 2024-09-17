using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByCommentGroupPostId
{
    public class GetReactByCommentGroupPostIdQuery : IQuery<GetReactByCommentGroupPostIdQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid CommentGroupPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
