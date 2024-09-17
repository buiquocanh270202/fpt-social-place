using Application.DTO.GetUserProfileDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetOtherUser
{
    public class GetOtherUserQueryResult
    {
        public GetOtherUserQueryResult()
        {
            WorkPlaces = new List<GetUserWorkPlaceDTO>();
            UserInterests = new List<GetUserInterers>();
            WebAffiliations = new List<GetUserWebAfflication>();
            AvataPhotos = new List<GetUserAvatar>();
        }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } //h
        public string LastName { get; set; } //h
        public DateTime BirthDay { get; set; }
        public string? AboutMe { get; set; }  //h
        public string? HomeTown { get; set; }
        public string? CoverImage { get; set; }  //h
        public string? UserNumber { get; set; }  //h
        public bool? ButtonFriend { get; set; }
        public List<GetUserWorkPlaceDTO>? WorkPlaces { get; set; }
        public List<GetUserInterers>? UserInterests { get; set; }
        public GetUserContactInfo? ContactInfo { get; set; }
        public GetUserGenderDTO? UserGender { get; set; }
        public GetUserRelationship? UserRelationship { get; set; }
        public List<GetUserWebAfflication>? WebAffiliations { get; set; }
        public List<GetUserAvatar>? AvataPhotos { get; set; }
    }
}
