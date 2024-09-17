using Application.DTO.UpdateSettingDTO;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateGroupSetting
{
    public class UpdateGroupSettingCommand : ICommand<UpdateGroupSettingCommandResult>
    {
        public UpdateGroupSettingCommand()
        {
            updateSettingDTOs = new List<UpdateGroupSettingDTO>();
        }
        public Guid? UserId { get; set; }
        public Guid GroupId { get; set; }
        public List<UpdateGroupSettingDTO> updateSettingDTOs { get; set; }
    }
}
