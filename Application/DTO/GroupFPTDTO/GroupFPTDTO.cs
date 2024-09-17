using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupFPTDTO
{
    public class GroupFPTDTO
    {
        public Guid GroupId { get; set; }
        public string? GroupNumber { get; set; }
        public string GroupName { get; set; } = null!;
        public string GroupDescription { get; set; } = null!;
        public Guid CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateAt { get; set; }
        public Guid GroupTypeId { get; set; }
        public string? CoverImage { get; set; }
        public Guid? GroupStatusId { get; set; }
        public bool? isJoined { get; set; }
    }
}
