using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.FindUserByName
{
    public class FindUserByNameQuery : IQuery<FindUserByNameQueryResult>
    {
        public Guid? UserId { get; set; }
        public string FindName { get; set; }
    }
}
