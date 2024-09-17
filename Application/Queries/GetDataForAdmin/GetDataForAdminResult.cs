using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetDataForAdmin
{
    public class GetDataForAdminResult
    {
        public int? NumberOfUser {  get; set; }
        public int? NumberOfActiveUser { get; set; }
        public int? NumberOfInactiveUser { get; set; }
        public int? NumberOfPost {  get; set; }
        public int? NumberOfGroup { get; set; }
    }
}
