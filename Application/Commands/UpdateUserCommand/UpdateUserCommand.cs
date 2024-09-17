using Application.DTO.CreateUserDTO;
using Application.DTO.UpdateUserDTO;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : ICommand<UpdateUserCommandResult>
    {
        public UpdateUserCommand()
        {
            UserInterests = new List<UpdateUserInteresDTO>();
            WorkPlaces = new List<UpdateUserWorkPlaceDTO>();
            WebAffiliations = new List<UpdateUserWebAffilicationDTO>();
        }
        public Guid? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public UpdateUserGenderDTO? UserGender { get; set; }
        public UpdateUserContactInforDTO? ContactInfo { get; set; }
        public UpdateUserRelationshipDTO? UserRelationship { get; set; }
        public string? AboutMe { get; set; }
        public string? HomeTown { get; set; }
        public string? CoverImage { get; set; }
        public string? Avataphoto { get; set; }
        public List<UpdateUserInteresDTO>? UserInterests { get; set; }
        public List<UpdateUserWorkPlaceDTO>? WorkPlaces { get; set; }
        public List<UpdateUserWebAffilicationDTO>? WebAffiliations { get; set; }
    }
}
