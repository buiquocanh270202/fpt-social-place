using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetWebAffilication
{
    public class GetWebAffilicationQuery : IQuery<List<GetWebAffilicationResult>>
    {
        public Guid? UserId { get; set; }
    }
}
