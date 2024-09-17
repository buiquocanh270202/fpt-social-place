using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByGroupPostId
{
    public class GetCommentByGroupPostIdQuery : IQuery<GetCommentByGroupPostIdQueryResult>
    {
        public Guid GroupPostId { get; set; }
        public string? Type { get; set; }
    }
}
