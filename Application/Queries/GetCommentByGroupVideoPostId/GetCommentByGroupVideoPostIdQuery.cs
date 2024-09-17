using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByGroupVideoPostId
{
    public class GetCommentByGroupVideoPostIdQuery : IQuery<GetCommentByGroupVideoPostIdQueryResult>    
    {
        public Guid GroupPostVideoId { get; set; }
        public string? Type { get; set; }
    }
}
