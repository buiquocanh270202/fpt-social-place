using Core.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateInterest
{
    public class CreateInterestCommand : ICommand<CreateInterestCommandResult>
    {
        public string InterestName { get; set; }
    }
}
