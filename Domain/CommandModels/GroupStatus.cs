using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupStatus
    {
        public GroupStatus()
        {
            GroupFpts = new HashSet<GroupFpt>();
            GroupPostPhotos = new HashSet<GroupPostPhoto>();
            GroupPostVideos = new HashSet<GroupPostVideo>();
            GroupPosts = new HashSet<GroupPost>();
            GroupSettingUses = new HashSet<GroupSettingUse>();
            GroupSharePosts = new HashSet<GroupSharePost>();
            GroupTagUseds = new HashSet<GroupTagUsed>();
        }

        public Guid GroupStatusId { get; set; }
        public string GroupStatusName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<GroupFpt> GroupFpts { get; set; }
        public virtual ICollection<GroupPostPhoto> GroupPostPhotos { get; set; }
        public virtual ICollection<GroupPostVideo> GroupPostVideos { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
        public virtual ICollection<GroupSettingUse> GroupSettingUses { get; set; }
        public virtual ICollection<GroupSharePost> GroupSharePosts { get; set; }
        public virtual ICollection<GroupTagUsed> GroupTagUseds { get; set; }
    }
}
