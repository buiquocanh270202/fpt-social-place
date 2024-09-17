﻿using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetChildGroupPost
{
    public class GetChildGroupPostQuery : IQuery<GetChildGroupPostResult>
    {
        public Guid? UserId { get; set; }
        public Guid GroupPostMediaId { get; set; }
    }
}
