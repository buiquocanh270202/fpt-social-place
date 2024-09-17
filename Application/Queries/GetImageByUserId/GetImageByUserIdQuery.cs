using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetImageByUserId
{
    public class GetImageByUserIdQuery : IQuery<GetImageByUserIdQueryResult>
    {
        public Guid UserId { get; set; }
        public Guid StrangerId { get; set; }
        public string? Type { get; set; }
        public int Page { get; set; } = 1;
    }
}
