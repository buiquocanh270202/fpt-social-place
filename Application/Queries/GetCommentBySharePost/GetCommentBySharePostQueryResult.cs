using Application.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentBySharePost
{
    public class GetCommentBySharePostQueryResult
    {
        public List<CommentSharePostDto>? Posts { get; set; }

    }
}
