using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByGroupPhotoPost
{
    public class GetReactByGroupPhotoPostQuery : IQuery< GetReactByGroupPhotoPostQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid GroupPostPhotoId { get; set; }
        public Guid UserId { get; set; }
    }
}
