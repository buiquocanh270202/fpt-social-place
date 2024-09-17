using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupDTO
{
    public class GetMemberInGroupDTO
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public string MemberName { get; set; }
        public string? Avata { get; set; }
        public Guid GroupRoleId { get; set; }
        public string GroupRoleName { get; set; }
        public bool IsFriend { get; set; }
        public bool IsRequested { get; set; }
        public bool SendFriendRequest { get; set; }
    }
}
