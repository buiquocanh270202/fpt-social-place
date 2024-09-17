using Application.DTO.CreateUserDTO;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.GetUserProfile
{
    public class UserProfileCommand :ICommand<UserProfileCommandResult>
    {
        public Guid? UserId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string? Email { get; set; }
        public string? FeId { get; set; }
        public string? RoleName { get; set; }
        public DateTime BirthDay { get; set; }
        public CreateUserGenderDTO? Gender { get; set; }
        public CreateUserContactInforDTO? ContactInfor { get; set; }
        public CreateUserRelationshipDTO? Relationship { get; set; }
        public string? AboutMe { get; set; }
        public string? HomeTown { get; set; }
        public string? CoverImage { get; set; }
        public string? UserNumber { get; set; }
        public string? Avataphoto { get; set; }
        public string? Campus { get; set; }
        public List<CreateUserSettingDTO>? UserSetting { get; set; }
        public List<CreateUserInteresDTO>? Interes { get; set; }
        public List<CreateUserWorkPlaceDTO>? WorkPlace { get; set; }
        public List<CreateUserWebAffilicationDTO>? WebAffilication { get; set; }
    }
}
