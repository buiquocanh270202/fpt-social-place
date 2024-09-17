using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllGroupSetting
{
    public class GetAllGroupSettingQueryResult
    {
        public Guid GroupSettingId { get; set; }
        public string GroupSettingName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
