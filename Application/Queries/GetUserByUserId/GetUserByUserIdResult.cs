using Application.DTO.GetUserProfileDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserByUserId
{
    public class GetUserByUserIdResult
    {
        public GetUserByUserIdResult()
        {
            WorkPlaces = new List<GetUserWorkPlaceDTO>();
            UserInterests = new List<GetUserInterers>();
            WebAffiliations = new List<GetUserWebAfflication>();
            AvataPhotos = new List<GetUserAvatar>();
        }
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
        public string? Campus { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid UserStatusId { get; set; }
        public string UserStatusName { get; set; }
        public List<GetUserWorkPlaceDTO>? WorkPlaces { get; set; }
        public List<GetUserInterers>? UserInterests { get; set;}
        public GetUserContactInfo? ContactInfo { get; set; }
        public GetUserGenderDTO? UserGender { get; set; }
        public GetUserRelationship? UserRelationship { get; set; }
        public List<GetUserWebAfflication>? WebAffiliations { get; set; }
        public List<GetUserAvatar>? AvataPhotos { get; set; }
    }
}
