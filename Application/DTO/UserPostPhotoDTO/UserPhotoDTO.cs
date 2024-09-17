using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UserPostPhotoDTO
{
    public class UserPhotoDTO
    {
        public Guid UserId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? UserPostId { get; set; }
        public string? PhotoUrl { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
    }
}
