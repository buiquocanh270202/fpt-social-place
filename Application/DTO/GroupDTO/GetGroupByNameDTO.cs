using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupDTO
{
    public class GetGroupByNameDTO
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string CoverIgame { get; set; }
        public Guid GroupTypeId { get; set; }
        public string GroupType { get; set; }
        public int MemberCount { get; set; }
    }
}
