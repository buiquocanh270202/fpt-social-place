using Application.DTO.UpdateSettingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserSettings
{
    public  class UpdateUserSettingsCommandResult
    {
        public UpdateUserSettingsCommandResult()
        {
            UserSettings = new List<UpdateSettingDTO>();
        }
        public Guid? UserId { get; set; }
        public List<UpdateSettingDTO> UserSettings { get; set; }
    }
}
