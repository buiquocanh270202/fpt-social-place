using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByPostId
{
    public class GetCommentByPostIdQuery : IQuery<GetCommentByPostIdQueryResult>
    {
        public Guid UserPostId { get; set; }
        public string? Type { get; set; }
    }
}
