using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserPostById
{
    public class GetUserPostByIdQuery : IQuery<GetUserPostByIdResult>
    {
        public Guid? UserId { get; set; }
        public Guid UserPostId { get; set; }
    }
}
