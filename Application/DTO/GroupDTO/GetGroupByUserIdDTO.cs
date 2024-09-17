using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupDTO
{
    public class GetGroupByUserIdDTO
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string? CoverImage { get; set; }
    }
}
