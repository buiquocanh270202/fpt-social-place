using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetRelationship
{
    public class GetRelationshipResult
    {
        public Guid RelationShipId {  get; set; }
        public string? RelationshipName { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
