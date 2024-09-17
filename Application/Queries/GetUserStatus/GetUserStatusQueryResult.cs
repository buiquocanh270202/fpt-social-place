using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserStatus
{
    public class GetUserStatusQueryResult
    {
        public Guid UserStatusId { get; set; }
        public string StatusName { get; set; }
    }
}
