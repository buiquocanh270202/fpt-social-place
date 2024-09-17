using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.FriendDTO
{
    public class GetUserChatDTO
    {
        public int ChatId  { get; set; }
        public Guid FriendId { get; set; }
        public string FriendName { get; set; }
        public int? ReactCount { get; set; }
        public int? MutualFriends { get; set; }
        public string? Avata { get; set; }
    }
}
