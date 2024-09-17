using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateCommentGroupSharePost
{
    public class UpdateCommentGroupSharePostCommandResult
    {
        public Guid CommentGroupSharePostId { get; set; }
        public Guid GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public List<CheckingBadWord.BannedWord> BannedWords { get; set; } = new List<CheckingBadWord.BannedWord>();
        public DateTime? CreatedDate { get; set; }
    }
}
