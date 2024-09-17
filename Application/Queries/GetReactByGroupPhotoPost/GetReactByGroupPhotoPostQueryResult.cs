using Application.DTO.ReactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByGroupPhotoPost
{
    public class GetReactByGroupPhotoPostQueryResult
    {
        public int SumOfReact {  get; set; }
        public bool? IsReact { get; set; }
        public List<ReactGroupPhotoPostDTO>? ListUserReact { get; set; }
        public List<ReactTypeCountDTO>? ListReact { get; set; }
    }
}
