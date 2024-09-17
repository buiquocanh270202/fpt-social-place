using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactUserPhotoPost
{
    public class CreateReactUserPhotoPostCommandResult
    {
        public Guid ReactPhotoPostId { get; set; }
        public Guid UserPostPhotoId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
