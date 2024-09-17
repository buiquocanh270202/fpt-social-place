
using Application.DTO.GetUserProfileDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserProfile
{
    public class GetUserQueryResult
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? FeId { get; set; }
        public DateTime BirthDay { get; set; }
        public string? AboutMe { get; set; }
        public string? HomeTown { get; set; }
        public string? CoverImage { get; set; }
        public string? UserNumber { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid? StatusId { get; set; }
        public string UserStatusName { get; set; }
        public GetUserContactInfo ContactInfo { get; set; }
        public Guid? GenderId { get; set; }
        public string GenderName { get; set; }
        public string LookingFor { get; set; }
        public string Relationship { get; set; }
        public List<GetUserWebAfflication> WebAffiliationUrl { get; set; }
        public List<GetUserAvatar> AvataPhotosUrl { get; set; }
    }
}
