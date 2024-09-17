using Application.Commands.CreateReactUserPost;
using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateNewReact
{
    public class CreateNewReactCommand : ICommand<CreateNewReactCommandResult>
    {
        public string ReactTypeName { get; set; }
    }
}
