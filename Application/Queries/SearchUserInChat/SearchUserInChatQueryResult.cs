using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.FriendDTO;

namespace Application.Queries.SearchUserInChat
{
    public class SearchUserInChatQueryResult
    {
        public SearchUserInChatQueryResult()
        {
            ListFriend = new List<GetUserChatDTO>();
            ListUserNotFriend = new List<GetUserChatDTO>();
        }
        public List<GetUserChatDTO> ListFriend { get; set; }
        public List<GetUserChatDTO> ListUserNotFriend { get; set; }
    }
}
