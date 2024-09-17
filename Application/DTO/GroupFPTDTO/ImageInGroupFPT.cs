using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GroupFPTDTO
{
    public class ImageInGroupFPT
    {
        public Guid GroupId { get; set; }
        public string? UrlImage { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
