using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.SuggestionGroup
{
    public class SuggestionGroupQuery : IQuery<SuggestionGroupQueryResult>
    {
        public Guid UserId { get; set; }

        public bool ShowAll { get; set; }
    }
}
