using Application.DTO.GroupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupByGroupId
{
    public class GetGroupByGroupIdQueryResult
    {
        public GetGroupByGroupIdQueryResult()
        {
            GroupMember = new List<GroupMemberDTO>();
            GroupSettings = new List<GroupSettingDTO>();
        }
        public Guid GroupId { get; set; }
        public string? GroupNumber { get; set; }
        public string GroupName { get; set; } = null!;
        public string GroupDescription { get; set; } = null!;
        public string GroupAdmin { get; set; }
        public string? CoverImage { get; set; }
        public Guid GroupTypeId { get; set; }
        public string? GroupTypeName { get; set; }
        public bool IsJoin { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCensor { get; set; }
        public bool isRequest { get; set; }
        public int MemberCount { get; set; }
        public DateTime? CreateAt { get; set; }
        public List<GroupSettingDTO> GroupSettings { get; set; }
        public List<GroupMemberDTO> GroupMember { get; set; }
    }
}
