using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReactUserVideoPost
{
    public class CreateReactUserVideoPostCommandResult
    {
        public Guid ReactVideoPostId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public Guid ReactTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
