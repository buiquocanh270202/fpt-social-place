using Application.Queries.GetCommentByGroupVideoPostId;
using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentbyGroupPhotoPostId
{
    public class GetCommentByGroupPhotoPostIdQuery : IQuery<GetCommentByGroupPhotoPostIdQueryResult>
    {
        public Guid GroupPostPhotoId { get; set; }
        public string? Type { get; set; }
    }
}
