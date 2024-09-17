using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class Photo
    {
        public Photo()
        {
            UserPostPhotos = new HashSet<UserPostPhoto>();
            UserPosts = new HashSet<UserPost>();
        }

        public Guid PhotoId { get; set; }
        public string PhotoUrl { get; set; } = null!;
        public string? PhotoBigUrl { get; set; }
        public string? PhotoSmallUrl { get; set; }
        public Guid UserId { get; set; }
        public string? PhotoNumber { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
        public virtual ICollection<UserPostPhoto> UserPostPhotos { get; set; }
        public virtual ICollection<UserPost> UserPosts { get; set; }
    }
}
