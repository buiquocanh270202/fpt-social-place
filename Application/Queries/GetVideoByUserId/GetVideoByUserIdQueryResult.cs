using Application.DTO.UserPostVideoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetVideoByUserId
{
    public class GetVideoByUserIdQueryResult
    {
        public List<UserVideoDTO>? Videos { get; set; }
    }
}
