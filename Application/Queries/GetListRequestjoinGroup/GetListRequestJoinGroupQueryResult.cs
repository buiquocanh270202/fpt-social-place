using Application.DTO.GroupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListRequestjoinGroup
{
    public class GetListRequestJoinGroupQueryResult
    {
        public GetListRequestJoinGroupQueryResult()
        {
            requestJoinGroups = new List<RequestJoinGroupDTO>();
        }
        public List<RequestJoinGroupDTO> requestJoinGroups { get; set; }
    }
}
