using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactCommentUserPost
{
    public class CreateReactCommentUserPostCommandResult
    {
        public Guid ReactCommentId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
