using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateRelationships
{
    public class CreateRelationShipCommandResult
    {
        public Guid RelationshipId { get; set; }
        public string RelationshipName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
