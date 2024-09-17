using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupDTO
{
    public class GetGroupStatusDTO
    {
        public Guid GroupStatusId { get; set; }
        public string GroupStatusName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
