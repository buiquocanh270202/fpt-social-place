using Application.DTO.CreateUserDTO;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.CreateUserInterest
{
    public class UserInterestCommand :ICommand<UserInterestCommandResult>
    {
        public Guid InterestId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
    }
}
