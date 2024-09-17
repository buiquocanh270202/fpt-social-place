using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateUserCommentVideoPost
{
    public class CreateUserCommentVideoPostCommandResult 
    {
        public Guid CommentVideoPostId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<CheckingBadWord.BannedWord> BannedWords { get; set; }
    }
}
