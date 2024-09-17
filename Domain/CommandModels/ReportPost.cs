using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class ReportPost
    {
        public Guid ReportPostId { get; set; }
        public Guid ReportTypeId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid ReportById { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public bool? ReportStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Processing { get; set; }
        public Guid? SharePostId { get; set; }
        public Guid? GroupSharePostId { get; set; }

        public virtual GroupPost? GroupPost { get; set; }
        public virtual GroupPostPhoto? GroupPostPhoto { get; set; }
        public virtual GroupPostVideo? GroupPostVideo { get; set; }
        public virtual GroupSharePost? GroupSharePost { get; set; }
        public virtual UserProfile ReportBy { get; set; } = null!;
        public virtual ReportType ReportType { get; set; } = null!;
        public virtual SharePost? SharePost { get; set; }
        public virtual UserPost? UserPost { get; set; }
        public virtual UserPostPhoto? UserPostPhoto { get; set; }
        public virtual UserPostVideo? UserPostVideo { get; set; }
    }
}
