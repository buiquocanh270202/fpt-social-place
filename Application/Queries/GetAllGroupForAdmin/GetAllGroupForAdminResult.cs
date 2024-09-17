using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllGroupForAdmin
{
    public class GetAllGroupForAdminResult
    {
        public List<GetAllGroup>? result;
        public int? totalPage;
    }
    
    public class GetAllGroup
    {
        public Guid GroupId { get; set; }
        public string? GroupName {  get; set; }
        public string? CoverImage { get; set; }
        public int? NumberOfMember {  get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
