using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactCommentUserPostVideo
{
    public class CreateReactCommentUserPostVideoCommandResult
    {
        public Guid ReactVideoPostCommentId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentVideoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
