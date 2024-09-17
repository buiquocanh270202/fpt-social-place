using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CheckUserExist
{
    public class CheckUserExistQueryResult
    {
        public StatusCodeEnum enumcode { get;set; }
        public string Message { get; set; }
        public Guid? UserId { get; set; }
        public string? UserNumber { get; set; }
        public string? Email { get; set; }
        public string? FeId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
