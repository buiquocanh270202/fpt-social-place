using Application.DTO.CommentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentbyGroupPhotoPostId
{
    public class GetCommentByGroupPhotoPostIdQueryResult
    {
        public List<GroupPhotoCommentDto>? Posts { get; set; }

    }
}
