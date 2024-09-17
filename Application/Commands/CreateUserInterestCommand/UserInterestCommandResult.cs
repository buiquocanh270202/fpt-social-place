using Application.DTO.CreateUserDTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateUserInterest
{
    public class UserInterestCommandResult 
    {
        public Guid UserInterestId { get; set; }
        public Guid InterestId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


    }
}
