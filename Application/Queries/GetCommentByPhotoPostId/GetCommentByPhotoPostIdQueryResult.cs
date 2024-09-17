using Application.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByPhotoPostId
{
    public class GetCommentByPhotoPostIdQueryResult
    {
        public List<CommentPhotoDto>? Posts { get; set; }

    }
}
