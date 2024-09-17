using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserIsBlocked
{
    public class GetUserIsBlockedQueryResult
    {
        public Guid UserBlockedId { get; set; }
        public string? Avata { get; set; }
        public string FullName { get; set; }
    }
}
