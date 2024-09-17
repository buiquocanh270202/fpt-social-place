using Application.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByGroupPostId
{
    public class GetCommentByGroupPostIdQueryResult
    {
        public List<GroupCommentDto>? Posts { get; set; }
    }
}
