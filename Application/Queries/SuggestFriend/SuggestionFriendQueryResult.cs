using Application.DTO.FriendDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.SuggestFriend
{
    public class SuggestionFriendQueryResult
    {
        public SuggestionFriendQueryResult()
        {
            AllFriend = new List<GetAllFriendDTO>();
        }
        public List<GetAllFriendDTO> AllFriend { get; set; }
        public int Count { get; set; }
    }
}
