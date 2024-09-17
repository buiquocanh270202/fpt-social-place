using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupTagUsed
    {
        public Guid TagId { get; set; }
        public Guid GroupId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public Guid? GroupStatusId { get; set; }

        public virtual GroupFpt Group { get; set; } = null!;
        public virtual GroupStatus? GroupStatus { get; set; }
        public virtual GroupTag Tag { get; set; } = null!;
    }
}
