using Application.Queries.GetUserProfile;
using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserProfile
{
    public class GetUserQuery : IQuery<GetUserQueryResult>
    {
        public string UserNumber { get; set; }
    }
}
