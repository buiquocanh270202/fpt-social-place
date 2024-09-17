using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupVideo
    {
        public GroupVideo()
        {
            GroupPostVideos = new HashSet<GroupPostVideo>();
            GroupPosts = new HashSet<GroupPost>();
        }

        public Guid GroupVideoId { get; set; }
        public string VideoUrl { get; set; } = null!;
        public Guid GroupId { get; set; }
        public string? GroupVideoNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual GroupFpt Group { get; set; } = null!;
        public virtual ICollection<GroupPostVideo> GroupPostVideos { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
    }
}
