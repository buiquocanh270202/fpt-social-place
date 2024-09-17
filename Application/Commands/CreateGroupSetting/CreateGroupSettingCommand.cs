using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupSetting
{
    public class CreateGroupSettingCommand : ICommand<CreateGroupSettingCommandResult>
    {
        public string GroupSettingName { get; set; }
    }
}
