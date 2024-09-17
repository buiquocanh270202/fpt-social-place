using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllGroupTag
{
    public class GetAllGroupTagQueryResult
    {
        public Guid TagId { get; set; }
        public string TagName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
