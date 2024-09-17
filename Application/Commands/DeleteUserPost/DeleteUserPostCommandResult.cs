using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteUserPost
{
    public class DeleteUserPostCommandResult 
    {
        public string? Message { get; set; }
        public bool IsDelete { get; set; }
    }
}
