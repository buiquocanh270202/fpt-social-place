﻿using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetGroupSettingByGroupId
{
    public class GetGroupSettingByGroupIdQuery : IQuery<List<GetGroupSettingByGroupIdQueryResult>>
    {
        public Guid? UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
