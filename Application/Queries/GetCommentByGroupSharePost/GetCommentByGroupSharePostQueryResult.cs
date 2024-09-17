using Application.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByGroupSharePost
{
    public class GetCommentByGroupSharePostQueryResult
    {
        public List<CommentGroupSharePostDto>? Posts { get; set; }

    }
}
