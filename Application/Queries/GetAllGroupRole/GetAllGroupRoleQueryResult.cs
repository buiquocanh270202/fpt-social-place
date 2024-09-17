using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllGroupRole
{
    public class GetAllGroupRoleQueryResult
    {
        public Guid GroupRoleId { get; set; }
        public string GroupRoleName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
