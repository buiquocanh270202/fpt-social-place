using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserPhotoPost
{
    public class UpdateUserPhotoPostCommandResult
    {
        public Guid UserPostPhotoId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid PhotoId { get; set; }
        public string? Content { get; set; }
        public string? UserPostPhotoNumber { get; set; }
        public Guid UserStatusId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostPosition { get; set; }
        public bool? IsBanned { get; set; }
        public List<CheckingBadWord.BannedWord>? BannedWords { get; set; }

    }
}
