using Application.DTO.FriendDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.FindUserByName
{
    public class FindUserByNameQueryResult
    {
        public FindUserByNameQueryResult()
        {
            ListFriend = new List<GetAllFriendDTO>();
            ListUserNotFriend = new List<GetAllFriendDTO>();
        }
       public List<GetAllFriendDTO> ListFriend { get; set; }
       public List<GetAllFriendDTO> ListUserNotFriend { get; set; }
    }
}
