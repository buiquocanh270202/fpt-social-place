using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupTag
    {
        public GroupTag()
        {
            GroupTagUseds = new HashSet<GroupTagUsed>();
        }

        public Guid TagId { get; set; }
        public string TagName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<GroupTagUsed> GroupTagUseds { get; set; }
    }
}
