using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactGroupPost
{
    public class CreateReactGroupPostCommandResult
    {
        public Guid ReactGroupPostId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public bool? IsReact { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
