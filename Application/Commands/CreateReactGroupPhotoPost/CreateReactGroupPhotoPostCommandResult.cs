using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactGroupPhotoPost
{
    public class CreateReactGroupPhotoPostCommandResult
    {
        public Guid ReactPhotoPostId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
