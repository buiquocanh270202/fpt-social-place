using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactGroupVideoPost
{
    public class CreateReactGroupVideoPostCommandResult
    {
        public Guid ReactVideoPostId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
