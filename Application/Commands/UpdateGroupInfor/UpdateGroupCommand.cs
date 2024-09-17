using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateGroupInfor
{
    public class UpdateGroupCommand : ICommand<UpdateGroupCommandResult>
    {
        public Guid? UserId { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string? Description { get; set; }
        public Guid? GroupTypeId { get; set; }
        public string? CoverImage { get; set; }
        public Guid? GroupStatusId { get; set; }
    }
}
