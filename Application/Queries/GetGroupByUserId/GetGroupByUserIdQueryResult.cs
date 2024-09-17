using Application.DTO.GroupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupByUserId
{
    public class GetGroupByUserIdQueryResult
    {
        public GetGroupByUserIdQueryResult()
        {
            ListGroupAdmin = new List<GetGroupByUserIdDTO>();
            ListGroupMember = new List<GetGroupByUserIdDTO>();
        }
        public List<GetGroupByUserIdDTO> ListGroupAdmin { get; set; }
        public List<GetGroupByUserIdDTO> ListGroupMember { get; set; }
    }
}
