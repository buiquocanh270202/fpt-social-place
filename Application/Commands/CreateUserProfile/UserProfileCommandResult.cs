using Application.DTO.CreateUserDTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.GetUserProfile
{
    public class UserProfileCommandResult 
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? FeId { get; set; }
        public DateTime BirthDay { get; set; }
        public string? AboutMe { get; set; }
        public string? HomeTown { get; set; }
        public string? CoverImage { get; set; }
        public string? UserNumber { get; set; }
        public CreateUserGenderDTO? Gender { get; set; }
        public CreateUserContactInforDTO? ContactInfor { get; set; }
        public CreateUserRelationshipDTO? Relationship { get; set; }
        public List<CreateUserSettingDTO>? UserSetting { get; set; }
        public List<CreateUserInteresDTO>? Interes { get; set; }
        public List<CreateUserWorkPlaceDTO>? WorkPlace { get; set; }
        public List<CreateUserWebAffilicationDTO>? WebAffilication { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public string? Avataphoto { get; set; }
        public Guid UserStatusId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

       
    }
}
