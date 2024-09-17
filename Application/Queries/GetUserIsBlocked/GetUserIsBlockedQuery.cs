﻿using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserIsBlocked
{
    public class GetUserIsBlockedQuery : IQuery<List<GetUserIsBlockedQueryResult>>
    {
        public Guid? UserId { get; set; }
    }
}
