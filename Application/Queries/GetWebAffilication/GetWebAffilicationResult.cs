using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetWebAffilication
{
    public class GetWebAffilicationResult
    {
        public Guid WebAffiliationId { get; set; }
        public string WebAffiliationUrl { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
