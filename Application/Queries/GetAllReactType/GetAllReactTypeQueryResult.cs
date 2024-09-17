using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllReactType
{
    public class GetAllReactTypeQueryResult
    {
        public Guid ReactTypeId { get; set; }
        public string ReactTypeName { get; set; } = null!;
    }
}
