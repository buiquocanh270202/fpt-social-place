using Application.DTO.ReactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UserPostPhotoDTO
{
    public class UserPostPhotoDTO
    {
        public Guid UserPostPhotoId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid PhotoId { get; set; }
        public string? Content { get; set; }
        public string? UserPostPhotoNumber { get; set; }
        public Guid UserStatusId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public virtual PhotoDTO Photo { get; set; } = null!;
        //public ReactCount ReactCount { get; set; }
    }

}
