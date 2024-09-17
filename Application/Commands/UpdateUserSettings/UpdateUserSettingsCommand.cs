using Application.DTO.UpdateSettingDTO;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserSettings
{
    public class UpdateUserSettingsCommand : ICommand<UpdateUserSettingsCommandResult>
    {
        public UpdateUserSettingsCommand()
        {
            UserSettings = new List<UpdateSettingDTO>();
        }
        public Guid? UserId { get; set; }
        public List<UpdateSettingDTO> UserSettings { get; set; }
    }
}
