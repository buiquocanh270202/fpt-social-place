using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByCommentGroupVideoId
{
    public class GetReactByCommentGroupVideoIdQuery : IQuery<GetReactByCommentGroupVideoIdQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid CommentGroupVideoPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
