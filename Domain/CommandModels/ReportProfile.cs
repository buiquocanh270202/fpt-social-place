using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReportProfile
    {
        public Guid ReportProfileId { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? UserId { get; set; }
        public Guid ReportById { get; set; }
        public bool? ReportStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Processing { get; set; }

        public virtual GroupFpt? Group { get; set; }
        public virtual UserProfile ReportBy { get; set; } = null!;
        public virtual ReportType ReportType { get; set; } = null!;
        public virtual UserProfile? User { get; set; }
    }
}
