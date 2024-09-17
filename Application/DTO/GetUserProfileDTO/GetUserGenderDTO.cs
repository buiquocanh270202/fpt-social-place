using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GetUserProfileDTO
{
    public class GetUserGenderDTO
    {
        public Guid UserGenderId { get; set; }
        public Guid? GenderId { get; set; }
        public string? GenderName { get; set; }
        public Guid? UserStatusId { get; set; }
        public string? StatusName { get; set; }
    }
}
