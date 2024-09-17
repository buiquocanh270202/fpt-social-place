using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GetUserProfileDTO
{
    public class GetUserRelationship
    {
        public Guid? UserRelationshipId { get; set; }
        public Guid? RelationshipId { get; set; }
        public string RelationshipName { get; set; }
        public Guid? UserStatusId { get; set; }
        public string? StatusName { get; set; }
    }
}
