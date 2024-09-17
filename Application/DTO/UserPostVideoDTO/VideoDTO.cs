using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UserPostVideoDTO
{
    public class VideoDTO
    {
        public Guid VideoId { get; set; }
        public string VideoUrl { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public string? VideoNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class VideoAddOnPost
    {
        public string? VideoUrl { get; set; }
        public int Position { get; set; }
    }

}
