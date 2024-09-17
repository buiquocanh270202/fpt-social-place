using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupSettingByGroupId
{
    public class GetGroupSettingByGroupIdQueryResult
    {
        public Guid GroupId { get; set; }
        public Guid GroupSettingId { get; set; }
        public string GroupSettingName { get; set; }
        public Guid GroupStatusId { get; set; }
        public string GroupStatusName { get; set; }
    }
}
