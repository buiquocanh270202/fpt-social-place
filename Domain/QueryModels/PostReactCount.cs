using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class PostReactCount
    {
        public Guid PostReactCountId { get; set; }
        public Guid? UserPostId { get; set; }
        public Guid? UserPostPhotoId { get; set; }
        public Guid? UserPostVideoId { get; set; }
        public int? ReactCount { get; set; }
        public int? CommentCount { get; set; }
        public int? ShareCount { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual UserPost? UserPost { get; set; }
        public virtual UserPostPhoto? UserPostPhoto { get; set; }
        public virtual UserPostVideo? UserPostVideo { get; set; }
    }
}
