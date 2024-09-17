using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactCommentGroupPost
{
    public class CreateReactCommentGroupPostPhotoCommandResult
    {
        public Guid ReactPhotoPostCommentId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentPhotoGroupPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
