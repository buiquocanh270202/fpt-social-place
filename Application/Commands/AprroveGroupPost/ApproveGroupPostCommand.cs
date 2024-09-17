using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.AprroveGroupPost
{
    public class ApproveGroupPostCommand : ICommand<ApproveGroupPostCommandResult>
    {
        public Guid UserId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupSharePostId { get; set; }

        public string Type { get; set; }
    }
}
