using Application.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByVideoPostId
{
    public class GetCommentByVideoPostIdQueryResult
    {
        public List<CommentVideoDto>? Posts { get; set; }

    }
}
