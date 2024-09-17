using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetNumberUserByDayForAdmin
{
    public class GetNumberUserByDayForAdminResult
    {
        public List<GetNumberUserByDay>? result {  get; set; }
        public int? totalPage { get; set; }
    }

    public class GetNumberUserByDay
    {
        public DateTime? Day { get; set; }
        public int NumberUser { get; set; }
    }
}
