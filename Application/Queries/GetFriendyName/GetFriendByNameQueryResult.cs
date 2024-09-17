using Application.DTO.FriendDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendyName
{
    public class GetFriendByNameQueryResult
    {
        public GetFriendByNameQueryResult()
        {
            getFriendByName = new List<GetAllFriendDTO>();
        }
        public List<GetAllFriendDTO> getFriendByName { get; set; }
        public int Count { get; set; }
    }
}
