using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactUserPost
{
    public class CreateReactUserPostCommandResult
    {
        public Guid ReactPostId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public bool? IsReact { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
