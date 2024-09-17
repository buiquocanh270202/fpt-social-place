using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.UpdateSharePost
{
    public class UpdateSharePostCommand : ICommand<UpdateSharePostCommandResult>
    {
        public Guid SharePostId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public string? Content { get; set; }
    }
}
