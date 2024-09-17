using Application.DTO.GroupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetMemberInGroup
{
    public class GetMemberInGroupQueryResult
    {
        public GetMemberInGroupQueryResult()
        {
            GroupAdmin = new List<GetMemberInGroupDTO>();
            GroupMangager = new List<GetMemberInGroupDTO>();
            GroupMember = new List<GetMemberInGroupDTO>();
        }
        public List<GetMemberInGroupDTO> GroupAdmin { get; set; }
        public List<GetMemberInGroupDTO> GroupMangager { get; set; }
        public List<GetMemberInGroupDTO> GroupMember { get; set; }
    }
}
