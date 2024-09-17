using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UserPostPhotoDTO
{
    public class PhotoDTO
    {
        public Guid PhotoId { get; set; }
        public string PhotoUrl { get; set; } = null!;
        public string? PhotoBigUrl { get; set; }
        public string? PhotoSmallUrl { get; set; }
        public Guid UserId { get; set; }
        public string? PhotoNumber { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class PhotoAddOnPost
    {
        public string? PhotoUrl { get; set; }
        public int Position { get; set; }
    }

}
