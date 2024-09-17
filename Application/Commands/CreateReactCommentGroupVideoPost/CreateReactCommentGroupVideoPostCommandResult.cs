using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactCommentGroupVideoPost
{
    public class CreateReactCommentGroupPostVideoCommandResult
    {
        public Guid ReactGroupVideoCommentId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid CommentGroupVideoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
