using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupPostDTO
{
    public class GroupVideoDTO
    {
        public Guid GroupVideoId { get; set; }
        public string VideoUrl { get; set; } = null!;
        public Guid GroupId { get; set; }
        public string? GroupVideoNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
