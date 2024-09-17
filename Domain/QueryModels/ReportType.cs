using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class ReportType
    {
        public ReportType()
        {
            ReportComments = new HashSet<ReportComment>();
            ReportPosts = new HashSet<ReportPost>();
            ReportProfiles = new HashSet<ReportProfile>();
        }

        public Guid ReportTypeId { get; set; }
        public string ReportTypeName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<ReportComment> ReportComments { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
        public virtual ICollection<ReportProfile> ReportProfiles { get; set; }
    }
}
