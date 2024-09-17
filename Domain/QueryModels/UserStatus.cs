using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            AvataPhotos = new HashSet<AvataPhoto>();
            ContactInfos = new HashSet<ContactInfo>();
            Notifications = new HashSet<Notification>();
            Photos = new HashSet<Photo>();
            SharePosts = new HashSet<SharePost>();
            UserGenders = new HashSet<UserGender>();
            UserInterests = new HashSet<UserInterest>();
            UserPostPhotos = new HashSet<UserPostPhoto>();
            UserPostVideos = new HashSet<UserPostVideo>();
            UserPosts = new HashSet<UserPost>();
            UserProfiles = new HashSet<UserProfile>();
            UserRelationships = new HashSet<UserRelationship>();
            UserSettings = new HashSet<UserSetting>();
            Videos = new HashSet<Video>();
            WebAffiliations = new HashSet<WebAffiliation>();
            WorkPlaces = new HashSet<WorkPlace>();
        }

        public Guid UserStatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<AvataPhoto> AvataPhotos { get; set; }
        public virtual ICollection<ContactInfo> ContactInfos { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<SharePost> SharePosts { get; set; }
        public virtual ICollection<UserGender> UserGenders { get; set; }
        public virtual ICollection<UserInterest> UserInterests { get; set; }
        public virtual ICollection<UserPostPhoto> UserPostPhotos { get; set; }
        public virtual ICollection<UserPostVideo> UserPostVideos { get; set; }
        public virtual ICollection<UserPost> UserPosts { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        public virtual ICollection<UserRelationship> UserRelationships { get; set; }
        public virtual ICollection<UserSetting> UserSettings { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<WebAffiliation> WebAffiliations { get; set; }
        public virtual ICollection<WorkPlace> WorkPlaces { get; set; }
    }
}
