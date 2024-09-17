using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.CreateUserDTO
{
        public class CreateUserGenderDTO
        {
            public Guid GenderId { get; set; }
        }
        public class CreateUserContactInforDTO
        {
            public string? SecondEmail { get; set; }
            public string? PrimaryNumber { get; set; }
            public string? SecondNumber { get; set; }
            public Guid? UserStatusId { get; set; }
        }
        public class CreateUserInteresDTO
        {
            public Guid? InterestId { get; set; }
        }
        public class CreateUserSettingDTO
        {
            public Guid? SettingId { get; set; }
        }

        public class CreateUserWebAffilicationDTO
        {
            public string? WebAffiliationUrl { get; set; }
        }

        public class CreateUserWorkPlaceDTO
        {
            public string? WorkPlaceName { get; set; }
        }
        public class CreateUserRelationshipDTO
        {
            public Guid? RelationshipId{ get; set; }
        }
}
