using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            AvataPhotos = new HashSet<AvataPhoto>();
            BlockUserUserIsBlockeds = new HashSet<BlockUser>();
            BlockUserUsers = new HashSet<BlockUser>();
            ChatAccounts = new HashSet<ChatAccount>();
            CommentGroupPosts = new HashSet<CommentGroupPost>();
            CommentGroupSharePosts = new HashSet<CommentGroupSharePost>();
            CommentGroupVideoPosts = new HashSet<CommentGroupVideoPost>();
            CommentPhotoGroupPosts = new HashSet<CommentPhotoGroupPost>();
            CommentPhotoPosts = new HashSet<CommentPhotoPost>();
            CommentPosts = new HashSet<CommentPost>();
            CommentSharePosts = new HashSet<CommentSharePost>();
            CommentVideoPosts = new HashSet<CommentVideoPost>();
            FriendFriendNavigations = new HashSet<Friend>();
            FriendUsers = new HashSet<Friend>();
            GroupFpts = new HashSet<GroupFpt>();
            GroupInvitationInviteds = new HashSet<GroupInvitation>();
            GroupInvitationInviters = new HashSet<GroupInvitation>();
            GroupMemberInvatedByNavigations = new HashSet<GroupMember>();
            GroupMemberUsers = new HashSet<GroupMember>();
            GroupPosts = new HashSet<GroupPost>();
            GroupSharePostSharedToUsers = new HashSet<GroupSharePost>();
            GroupSharePostUserShareds = new HashSet<GroupSharePost>();
            GroupSharePostUsers = new HashSet<GroupSharePost>();
            NotificationSenders = new HashSet<Notification>();
            NotificationUsers = new HashSet<Notification>();
            Photos = new HashSet<Photo>();
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
            ReportComments = new HashSet<ReportComment>();
            ReportPosts = new HashSet<ReportPost>();
            ReportProfileReportBies = new HashSet<ReportProfile>();
            ReportProfileUsers = new HashSet<ReportProfile>();
            SharePostSharedToUsers = new HashSet<SharePost>();
            SharePostUserShareds = new HashSet<SharePost>();
            SharePostUsers = new HashSet<SharePost>();
            UserChatChatWiths = new HashSet<UserChat>();
            UserChatUsers = new HashSet<UserChat>();
            UserInterests = new HashSet<UserInterest>();
            UserPosts = new HashSet<UserPost>();
            UserSettings = new HashSet<UserSetting>();
            Videos = new HashSet<Video>();
            WebAffiliations = new HashSet<WebAffiliation>();
            WorkPlaces = new HashSet<WorkPlace>();
            Followeds = new HashSet<UserProfile>();
            Followers = new HashSet<UserProfile>();
        }

        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => $"{FirstName} {LastName}";
        public string? Email { get; set; }
        public string? FeId { get; set; }
        public DateTime BirthDay { get; set; }
        public string? AboutMe { get; set; }
        public string? HomeTown { get; set; }
        public string? CoverImage { get; set; }
        public string? UserNumber { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public Guid UserStatusId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Campus { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
        public virtual ContactInfo? ContactInfo { get; set; }
        public virtual UserGender? UserGender { get; set; }
        public virtual UserRelationship? UserRelationship { get; set; }
        public virtual ICollection<AvataPhoto> AvataPhotos { get; set; }
        public virtual ICollection<BlockUser> BlockUserUserIsBlockeds { get; set; }
        public virtual ICollection<BlockUser> BlockUserUsers { get; set; }
        public virtual ICollection<ChatAccount> ChatAccounts { get; set; }
        public virtual ICollection<CommentGroupPost> CommentGroupPosts { get; set; }
        public virtual ICollection<CommentGroupSharePost> CommentGroupSharePosts { get; set; }
        public virtual ICollection<CommentGroupVideoPost> CommentGroupVideoPosts { get; set; }
        public virtual ICollection<CommentPhotoGroupPost> CommentPhotoGroupPosts { get; set; }
        public virtual ICollection<CommentPhotoPost> CommentPhotoPosts { get; set; }
        public virtual ICollection<CommentPost> CommentPosts { get; set; }
        public virtual ICollection<CommentSharePost> CommentSharePosts { get; set; }
        public virtual ICollection<CommentVideoPost> CommentVideoPosts { get; set; }
        public virtual ICollection<Friend> FriendFriendNavigations { get; set; }
        public virtual ICollection<Friend> FriendUsers { get; set; }
        public virtual ICollection<GroupFpt> GroupFpts { get; set; }
        public virtual ICollection<GroupInvitation> GroupInvitationInviteds { get; set; }
        public virtual ICollection<GroupInvitation> GroupInvitationInviters { get; set; }
        public virtual ICollection<GroupMember> GroupMemberInvatedByNavigations { get; set; }
        public virtual ICollection<GroupMember> GroupMemberUsers { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
        public virtual ICollection<GroupSharePost> GroupSharePostSharedToUsers { get; set; }
        public virtual ICollection<GroupSharePost> GroupSharePostUserShareds { get; set; }
        public virtual ICollection<GroupSharePost> GroupSharePostUsers { get; set; }
        public virtual ICollection<Notification> NotificationSenders { get; set; }
        public virtual ICollection<Notification> NotificationUsers { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
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
        public virtual ICollection<ReportComment> ReportComments { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
        public virtual ICollection<ReportProfile> ReportProfileReportBies { get; set; }
        public virtual ICollection<ReportProfile> ReportProfileUsers { get; set; }
        public virtual ICollection<SharePost> SharePostSharedToUsers { get; set; }
        public virtual ICollection<SharePost> SharePostUserShareds { get; set; }
        public virtual ICollection<SharePost> SharePostUsers { get; set; }
        public virtual ICollection<UserChat> UserChatChatWiths { get; set; }
        public virtual ICollection<UserChat> UserChatUsers { get; set; }
        public virtual ICollection<UserInterest> UserInterests { get; set; }
        public virtual ICollection<UserPost> UserPosts { get; set; }
        public virtual ICollection<UserSetting> UserSettings { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<WebAffiliation> WebAffiliations { get; set; }
        public virtual ICollection<WorkPlace> WorkPlaces { get; set; }

        public virtual ICollection<UserProfile> Followeds { get; set; }
        public virtual ICollection<UserProfile> Followers { get; set; }
    }
}
