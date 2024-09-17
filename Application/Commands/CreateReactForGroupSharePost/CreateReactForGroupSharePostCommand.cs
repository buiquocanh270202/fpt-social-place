using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.CreateReactForGroupSharePost
{
    public class CreateReactForGroupSharePostCommand : ICommand<CreateReactForGroupSharePostCommandResult>
    {
        public Guid GroupSharePostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
    }
}
