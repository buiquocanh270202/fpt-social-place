using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateAdminProfile
{
    public class CreateAdminProfileCommand : ICommand<CreateAdminProfileCommandResult>
    {
        public Guid? AdminId { get; set; }
        public string? RoleName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
