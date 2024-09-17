using Application.Queries.GetCommentByPostId;
using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByVideoPostId
{
    public class GetCommentByVideoPostIdQuery : IQuery<GetCommentByVideoPostIdQueryResult>
    {
        public Guid UserPostVideoId { get; set; }
        public string? Type { get; set; }
    }
}
