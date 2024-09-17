using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Commands.CreateCommentForGroupSharePost
{
    public class CreateCommentForGroupSharePostCommand : ICommand<CreateCommentForGroupSharePostCommandResult>
    {
        public Guid GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
