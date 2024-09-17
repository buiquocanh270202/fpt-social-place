using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactForSharePost
{
    public class CreateReactForSharePostCommandResult
    {
        public Guid ReactSharePostId { get; set; }
        public Guid SharePostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
