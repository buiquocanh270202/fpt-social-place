using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateAdminProfile
{
    public class CreateAdminProfileCommandResult
    {
        public string Message { get; set; }
        public string FullName { get; set; }
        public string Email { get; set;}
    }
}
