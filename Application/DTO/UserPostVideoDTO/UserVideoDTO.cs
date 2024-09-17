using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UserPostVideoDTO
{
    public class UserVideoDTO
    {
        public Guid UserId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? UserPostId { get; set; }
        public string? VideoUrl { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
    }
}
