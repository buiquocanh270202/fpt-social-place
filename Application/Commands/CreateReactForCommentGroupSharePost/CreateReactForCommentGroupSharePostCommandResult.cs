using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactForCommentGroupSharePost
{
    public class CreateReactForCommentGroupSharePostCommandResult
    {
        public Guid ReactGroupSharePosCommentId { get; set; }
        public Guid GroupSharePostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public Guid? CommentGroupSharePostId { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
