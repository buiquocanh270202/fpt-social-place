using Application.DTO.CreateUserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserCommand
{
    public class UpdateUserCommandResult
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public CreateUserGenderDTO? Gender { get; set; }
        public CreateUserContactInforDTO? ContactInfor { get; set; }
        public CreateUserRelationshipDTO? Relationship { get; set; }
        public string? AboutMe { get; set; }
        public string? HomeTown { get; set; }
        public string? CoverImage { get; set; }
        public string? Avataphoto { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CreateUserSettingDTO>? UserSetting { get; set; }
        public List<CreateUserInteresDTO>? Interes { get; set; }
        public List<CreateUserWorkPlaceDTO>? WorkPlace { get; set; }
        public List<CreateUserWebAffilicationDTO>? WebAffilication { get; set; }
    }
}
