using Application.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByGroupVideoPostId
{
    public class GetCommentByGroupVideoPostIdQueryResult
    {
        public List<GroupVideoCommentDto>? Posts { get; set; }

    }
}
