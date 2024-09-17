using Application.DTO.CreateUserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserOnChat
{
    public class GetUserOnChatQueryResult
    {
        public GetUserOnChatQueryResult()
        {
            ListFriend = new List<User>();
            OtherUser = new List<User>();
        }
        public List<User> ListFriend { get; set; }
        public List<User> OtherUser { get; set; }
    }
}
