using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetInvatedJoinGroup
{
    public class GetInvatedJoinGroupQueryResult
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string? CoverImage { get; set; }
        public Guid InvatedBy { get; set; }
        public string InvatedByName { get; set; }
        public string? InvatedByAvata { get; set; }
    }
}
