using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteCommentUserPost
{
    public class DeleteCommentUserPostCommandResult
    {
        public string? Message {  get; set; }
        public bool IsDelete { get; set; }
    }
}
