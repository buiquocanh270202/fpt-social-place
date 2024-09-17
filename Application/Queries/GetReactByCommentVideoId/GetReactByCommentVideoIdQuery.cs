using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByCommentVideoId
{
    public class GetReactByCommentVideoIdQuery : IQuery<GetReactByCommentVideoIdQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid CommentVideoPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
