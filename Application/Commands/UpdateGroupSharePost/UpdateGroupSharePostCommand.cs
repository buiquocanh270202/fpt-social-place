using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.UpdateGroupSharePost
{
    public class UpdateGroupSharePostCommand : ICommand<UpdateGroupSharePostCommandResult>
    {
        public Guid GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
    }
}
