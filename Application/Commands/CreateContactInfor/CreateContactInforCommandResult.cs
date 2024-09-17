using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateContactInfor
{
    public class CreateContactInforCommandResult
    {
        public Guid ContactInfoId { get; set; }
        public string? SecondEmail { get; set; }
        public string PrimaryNumber { get; set; } = null!;
        public string? SecondNumber { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
