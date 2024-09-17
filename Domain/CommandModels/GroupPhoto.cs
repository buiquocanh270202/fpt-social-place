using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupPhoto
    {
        public GroupPhoto()
        {
            GroupPostPhotos = new HashSet<GroupPostPhoto>();
            GroupPosts = new HashSet<GroupPost>();
        }

        public Guid GroupPhotoId { get; set; }
        public string PhotoUrl { get; set; } = null!;
        public string? PhotoBigUrl { get; set; }
        public string? GroupPhotoNumber { get; set; }
        public string? PhotoSmallUrl { get; set; }
        public Guid GroupId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual GroupFpt Group { get; set; } = null!;
        public virtual ICollection<GroupPostPhoto> GroupPostPhotos { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
    }
}
