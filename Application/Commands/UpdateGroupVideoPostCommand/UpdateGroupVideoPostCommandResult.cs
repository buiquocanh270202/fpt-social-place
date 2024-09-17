using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateGroupVideoPostCommand
{
    public class UpdateGroupVideoPostCommandResult
    {
        public Guid GroupPostVideoId { get; set; }
        public Guid GroupPostId { get; set; }
        public string? Content { get; set; }
        public string? GroupPostVideoNumber { get; set; }
        public Guid GroupStatusId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public bool? IsBanned { get; set; }
        public List<CheckingBadWord.BannedWord>? BannedWords { get; set; }
    }
}
