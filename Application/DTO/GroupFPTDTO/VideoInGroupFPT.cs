using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupFPTDTO
{
    public class VideoInGroupFPT
    {
        public Guid GroupId { get; set; }
        public string? UrlVideo { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
