using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllGroupStatus
{
    public class GetGroupStatusQueryResult
    {
        public Guid GroupStatusId { get; set; }
        public string GroupStatusName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
