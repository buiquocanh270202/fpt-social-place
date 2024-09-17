using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByPhotoPost
{
    public class GetReactByPhotoPostQuery : IQuery< GetReactByPhotoPostQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid UserPostPhotoId { get; set; }
        public Guid UserId { get; set; }
    }
}
