using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendRequested
{
    public class GetAllFriendRequestQueryResult
    {
        public bool IsRequested { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }    
        public string Avata { get; set; }
    }
}
