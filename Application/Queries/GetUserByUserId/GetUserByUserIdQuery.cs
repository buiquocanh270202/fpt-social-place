using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserByUserId
{
    public class GetUserByUserIdQuery : IQuery<GetUserByUserIdResult>
    {
        public Guid? UserId { get; set; }
    }
}
