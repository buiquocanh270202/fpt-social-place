using Application.DTO.GroupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupByName
{
    public class GetGroupByNameQueryResult
    {
        public GetGroupByNameQueryResult()
        {
            GroupDontJoin = new List<GetGroupByNameDTO>();
            GroupJoined = new List<GetGroupByNameDTO>();
        }
        public List<GetGroupByNameDTO> GroupJoined { get; set;}
        public List<GetGroupByNameDTO> GroupDontJoin { get; set; }
    }
}
