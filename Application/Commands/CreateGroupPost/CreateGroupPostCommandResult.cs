using Application.Services;
using Domain.CommandModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupPost
{
    public class CreateGroupPostCommandResult
    {
        public string GroupPostId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? Content { get; set; }
        public string? GroupPostNumber { get; set; }
        public string GroupStatusId { get; set; } = null!;
        public bool? IsHide { get; set; }
        public List<CheckingBadWord.BannedWord>? BannedWords { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
