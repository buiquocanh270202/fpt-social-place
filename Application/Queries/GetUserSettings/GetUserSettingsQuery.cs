using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserSettings
{
    public class GetUserSettingsQuery : IQuery<GetUserSettingsQueryResult>
    {
        public Guid? UserId { get; set; }
    }
}
