﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListFriendToInvate
{
    public class GetListFriendToInvateResult
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string? Avata { get; set; }

    }
}
