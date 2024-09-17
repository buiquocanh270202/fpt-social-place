using Application.DTO.ReactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UserPostVideoDTO
{
    public class UserPostVideoDTO
    {
        public Guid UserPostVideoId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid VideoId { get; set; }
        public string? Content { get; set; }
        public string? UserPostVideoNumber { get; set; }
        public Guid UserStatusId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public virtual VideoDTO Video { get; set; } = null!;
        //public ReactCount ReactCount { get; set; }
    }

}
