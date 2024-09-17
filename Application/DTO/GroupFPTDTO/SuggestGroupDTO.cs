using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupFPTDTO
{
    public class SuggestGroupDTO
    {
        public Guid GroupId { get; set; }
        public string? GroupName { get; set; }
        public int NumberOfMember { get; set; }
        public string? GroupStatus { get; set; }
        public string? CoverImage { get; set; }
    }
}
