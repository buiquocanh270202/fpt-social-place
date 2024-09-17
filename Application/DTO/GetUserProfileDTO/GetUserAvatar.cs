using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GetUserProfileDTO
{
    public class GetUserAvatar
    {
        public Guid? AvataPhotosId { get; set; }
        public string? AvataPhotosUrl { get; set; } = null!;
        public Guid? UserStatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsUsed { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
