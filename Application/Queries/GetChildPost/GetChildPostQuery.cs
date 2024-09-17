using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetChildPost
{
    public class GetChildPostQuery : IQuery<GetChildPostResult>
    {
        public Guid? UserId { get; set; }
        public Guid UserPostMediaId { get; set; }
    }
}
