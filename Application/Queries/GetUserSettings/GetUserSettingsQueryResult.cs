using Application.DTO.UpdateSettingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserSettings
{
    public class GetUserSettingsQueryResult
    {
        public GetUserSettingsQueryResult()
        {
            Usersettings = new List<UserSettingDTO>();
        }
        public Guid? UserId { get; set; }
        public List<UserSettingDTO> Usersettings { get; set; }
    }
}
