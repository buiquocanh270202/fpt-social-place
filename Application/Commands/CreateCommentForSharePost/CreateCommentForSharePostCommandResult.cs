using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateCommentForSharePost
{
    public class CreateCommentForSharePostCommandResult
    {
        public Guid CommentSharePostId { get; set; }
        public Guid SharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? ListNumber { get; set; }
        public int? LevelCmt { get; set; }
        public bool? IsHide { get; set; }
        public List<CheckingBadWord.BannedWord> BannedWords { get; set; } = new List<CheckingBadWord.BannedWord>();
        public DateTime? CreateDate { get; set; }
        public bool? IsBanned { get; set; }
    }
}
