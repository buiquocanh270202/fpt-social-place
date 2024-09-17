using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GetUserProfileDTO
{
    public class GetUserWorkPlaceDTO
    {
        public Guid? WorkPlaceId { get; set; }
        public string? WorkPlaceName { get; set; } 
        public Guid? UserStatusId { get; set; }
    }
}
