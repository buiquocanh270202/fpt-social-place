using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupPostDTO
{
    public class GroupPhotoDTO
    {
        public Guid GroupPhotoId { get; set; }
        public string PhotoUrl { get; set; } = null!;
        public string? PhotoBigUrl { get; set; }
        public string? GroupPhotoNumber { get; set; }
        public string? PhotoSmallUrl { get; set; }
        public Guid GroupId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
