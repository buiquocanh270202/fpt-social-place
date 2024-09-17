using Application.DTO.CommentDTO;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByPostId
{
    public class GetCommentByPostIdQueryResult
    {
        public List<CommentDto>? Posts { get; set; }
    }
}
