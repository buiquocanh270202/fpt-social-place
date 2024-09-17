using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.UpdateUserDTO
{
    public class UpdateUserGenderDTO
    {
        public Guid? GenderId { get; set; }
        public Guid? UserStatusId { get; set; }
    }
    public class UpdateUserContactInforDTO
    {
        public string? SecondEmail { get; set; }
        public string? PrimaryNumber { get; set; }
        public string? SecondNumber { get; set; }
        public Guid? UserStatusId { get; set; }
    }
    public class UpdateUserInteresDTO
    {
        public Guid? InterestId { get; set; }
        public Guid? UserStatusId { get; set; }
    }
    public class UpdateUserSettingDTO
    {
        public Guid? SettingId { get; set; }
        public Guid? UserStatusId { get; set; }
    }

    public class UpdateUserWebAffilicationDTO
    {
        public Guid? WebAffiliationId { get; set; }
        public string? WebAffiliationUrl { get; set; }
        public Guid? UserStatusId { get; set; }
    }

    public class UpdateUserWorkPlaceDTO
    {
        public Guid? WorkPlaceId { get; set; }
        public string? WorkPlaceName { get; set; }
        public Guid? UserStatusId { get; set; }
    }
    public class UpdateUserRelationshipDTO
    {
        public Guid? RelationshipId { get; set; }
        public Guid? UserStatusId { get; set; }
    }
}
