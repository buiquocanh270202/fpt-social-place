using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupDTO
{
    public class SuggestionGroupDTO
    {
        public SuggestionGroupDTO()
        {
            ListFriend = new List<ProfileUser>();
        }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string CoverIgame { get; set; }
        public Guid GroupTypeId { get; set; }
        public string GroupType { get; set; }
        public int MemberCount { get; set; }
        public List<ProfileUser> ListFriend { get; set; }
    }

    public class ProfileUser
    {
        public string UserName { get; set; }
        public string Avata { get; set; }
    }
}
