using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateNewReact
{
    public class CreateNewReactCommandResult
    {
        public Guid ReactTypeId { get; set; }
        public string ReactTypeName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
