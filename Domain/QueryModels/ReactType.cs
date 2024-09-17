using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class ReactType
    {
        public ReactType()
        {
            ReactComments = new HashSet<ReactComment>();
            ReactGroupCommentPosts = new HashSet<ReactGroupCommentPost>();
            ReactGroupPhotoPostComments = new HashSet<ReactGroupPhotoPostComment>();
            ReactGroupPhotoPosts = new HashSet<ReactGroupPhotoPost>();
            ReactGroupPosts = new HashSet<ReactGroupPost>();
            ReactGroupSharePostComments = new HashSet<ReactGroupSharePostComment>();
            ReactGroupSharePosts = new HashSet<ReactGroupSharePost>();
            ReactGroupVideoPostComments = new HashSet<ReactGroupVideoPostComment>();
            ReactGroupVideoPosts = new HashSet<ReactGroupVideoPost>();
            ReactPhotoPostComments = new HashSet<ReactPhotoPostComment>();
            ReactPhotoPosts = new HashSet<ReactPhotoPost>();
            ReactPosts = new HashSet<ReactPost>();
            ReactSharePostComments = new HashSet<ReactSharePostComment>();
            ReactSharePosts = new HashSet<ReactSharePost>();
            ReactVideoPostComments = new HashSet<ReactVideoPostComment>();
            ReactVideoPosts = new HashSet<ReactVideoPost>();
        }

        public Guid ReactTypeId { get; set; }
        public string ReactTypeName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<ReactComment> ReactComments { get; set; }
        public virtual ICollection<ReactGroupCommentPost> ReactGroupCommentPosts { get; set; }
        public virtual ICollection<ReactGroupPhotoPostComment> ReactGroupPhotoPostComments { get; set; }
        public virtual ICollection<ReactGroupPhotoPost> ReactGroupPhotoPosts { get; set; }
        public virtual ICollection<ReactGroupPost> ReactGroupPosts { get; set; }
        public virtual ICollection<ReactGroupSharePostComment> ReactGroupSharePostComments { get; set; }
        public virtual ICollection<ReactGroupSharePost> ReactGroupSharePosts { get; set; }
        public virtual ICollection<ReactGroupVideoPostComment> ReactGroupVideoPostComments { get; set; }
        public virtual ICollection<ReactGroupVideoPost> ReactGroupVideoPosts { get; set; }
        public virtual ICollection<ReactPhotoPostComment> ReactPhotoPostComments { get; set; }
        public virtual ICollection<ReactPhotoPost> ReactPhotoPosts { get; set; }
        public virtual ICollection<ReactPost> ReactPosts { get; set; }
        public virtual ICollection<ReactSharePostComment> ReactSharePostComments { get; set; }
        public virtual ICollection<ReactSharePost> ReactSharePosts { get; set; }
        public virtual ICollection<ReactVideoPostComment> ReactVideoPostComments { get; set; }
        public virtual ICollection<ReactVideoPost> ReactVideoPosts { get; set; }
    }
}
