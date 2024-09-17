using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class GroupPostReactCount
    {
        public Guid GroupPostReactCountId { get; set; }
        public Guid? GroupPostId { get; set; }
        public Guid? GroupPostPhotoId { get; set; }
        public Guid? GroupPostVideoId { get; set; }
        public int? ReactCount { get; set; }
        public int? CommentCount { get; set; }
        public int? ShareCount { get; set; }

        public virtual GroupPost? GroupPost { get; set; }
        public virtual GroupPostPhoto? GroupPostPhoto { get; set; }
        public virtual GroupPostVideo? GroupPostVideo { get; set; }
    }
}
