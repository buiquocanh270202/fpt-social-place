using Application.DTO.ReactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactCommentDetail
{
    public class GetReactCommentDetailQueryResult
    {
        public List<ReactDetailDTO>? ListUserReact { get; set; }
    }
}
