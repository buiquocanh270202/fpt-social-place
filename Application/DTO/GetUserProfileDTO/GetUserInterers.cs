using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.GetUserProfileDTO
{
    public class GetUserInterers
    {
        public Guid? UserInterestId { get; set; }
        public string? InteresName { get; set; }
        public Guid? InterestId { get; set; }
        public Guid? UserId { get; set; }
        public string? StatusName { get; set; }
        public Guid? UserStatusId { get; set; }
    }
}
