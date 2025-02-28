﻿using Core.CQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetReactByCommentSharePostId
{
    public class GetReactByCommentSharePostQuery : IQuery<GetReactByCommentSharePostQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public Guid CommentSharePostId { get; set; }
        public Guid UserId { get; set; }

    }
}
