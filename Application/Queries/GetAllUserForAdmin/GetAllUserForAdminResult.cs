using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllUserForAdmin
{
    public class GetAllUserForAdminResult
    {
        public List<GetAllUser>? result {  get; set; }
        public int? totalPage { get; set; }
    }

    public class GetAllUser
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
