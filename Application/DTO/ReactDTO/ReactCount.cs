using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ReactDTO
{
    public class ReactCount
    {
        public int? ReactNumber { get; set; }
        public int? CommentNumber { get; set; }
        public int? ShareNumber{ get; set; }
        public bool? IsReact { get; set; }
        public ReactTypeCountDTO? UserReactType { get; set; }
        public List<ReactTypeCountDTO>? Top2React { get; set; }
    }
}
