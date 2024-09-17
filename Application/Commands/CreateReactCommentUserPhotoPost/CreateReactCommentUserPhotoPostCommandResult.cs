using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactCommentUserPost
{
    public class CreateReactCommentUserPostPhotoCommandResult
    {
        public Guid ReactPhotoPostCommentId { get; set; }
        public Guid UserPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentPhotoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
