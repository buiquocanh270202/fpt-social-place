using Application.DTO.GroupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListMemberRole
{
    public class GetListMemberRoleQueryResult
    {
        public GetListMemberRoleQueryResult()
        {
            GroupAdmin = new List<GroupMemberDTO>();
            GroupMangager = new List<GroupMemberDTO>();
            GroupMember = new List<GroupMemberDTO>();
        }
        public List<GroupMemberDTO> GroupAdmin { get; set; }
        public List<GroupMemberDTO> GroupMangager { get; set; }
        public List<GroupMemberDTO> GroupMember { get; set; }
    }
}
