using Application.Queries.GetCommentByPostId;
using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByPhotoPostId
{
    public class GetCommentByPhotoPostIdQuery : IQuery<GetCommentByPhotoPostIdQueryResult>
    {
        public Guid UserPostPhotoId { get; set; }
        public string? Type { get; set; }
    }
}
