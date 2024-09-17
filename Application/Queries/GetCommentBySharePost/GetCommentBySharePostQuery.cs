using Core.CQRS.Command;
using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Queries.GetCommentBySharePost
{
    public class GetCommentBySharePostQuery : IQuery<GetCommentBySharePostQueryResult>
    {
        public Guid? SharePostId { get; set; }
        public string? Type { get; set; }
    }
}
