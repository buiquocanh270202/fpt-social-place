using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupDTO
{
    public class GroupSettingDTO
    {
        public Guid GroupSettingId { get; set; }
        public string GroupSettingName { get; set; } = null!;
        public Guid GroupStatusId { get; set; }
        public string GroupStatusName { get; set; }
    }
}
