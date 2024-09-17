using Application.DTO.ReactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByCommentGroupPostId
{
    public class GetReactByCommentGroupPostIdQueryResult
    {
        public int SumOfReact { get; set; }
        public bool? IsReact { get; set; }
        public List<ReactGroupCommentDTO>? ListCommentReact { get; set; }
        public List<ReactTypeCountDTO>? ListReact { get; set;}
    }
}
