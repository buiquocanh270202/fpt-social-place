using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Domain.CommandModels
{
    public partial class fptforumCommandContext : DbContext
    {
        public fptforumCommandContext()
        {
        }

        public fptforumCommandContext(DbContextOptions<fptforumCommandContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminProfile> AdminProfiles { get; set; } = null!;
        public virtual DbSet<AvataPhoto> AvataPhotos { get; set; } = null!;
        public virtual DbSet<BlockType> BlockTypes { get; set; } = null!;
        public virtual DbSet<BlockUser> BlockUsers { get; set; } = null!;
        public virtual DbSet<ChatAccount> ChatAccounts { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<CommentGroupPost> CommentGroupPosts { get; set; } = null!;
        public virtual DbSet<CommentGroupSharePost> CommentGroupSharePosts { get; set; } = null!;
        public virtual DbSet<CommentGroupVideoPost> CommentGroupVideoPosts { get; set; } = null!;
        public virtual DbSet<CommentPhotoGroupPost> CommentPhotoGroupPosts { get; set; } = null!;
        public virtual DbSet<CommentPhotoPost> CommentPhotoPosts { get; set; } = null!;
        public virtual DbSet<CommentPost> CommentPosts { get; set; } = null!;
        public virtual DbSet<CommentSharePost> CommentSharePosts { get; set; } = null!;
        public virtual DbSet<CommentVideoPost> CommentVideoPosts { get; set; } = null!;
        public virtual DbSet<ContactInfo> ContactInfos { get; set; } = null!;
        public virtual DbSet<Friend> Friends { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<GroupFpt> GroupFpts { get; set; } = null!;
        public virtual DbSet<GroupInvitation> GroupInvitations { get; set; } = null!;
        public virtual DbSet<GroupMember> GroupMembers { get; set; } = null!;
        public virtual DbSet<GroupPhoto> GroupPhotos { get; set; } = null!;
        public virtual DbSet<GroupPost> GroupPosts { get; set; } = null!;
        public virtual DbSet<GroupPostPhoto> GroupPostPhotos { get; set; } = null!;
        public virtual DbSet<GroupPostReactCount> GroupPostReactCounts { get; set; } = null!;
        public virtual DbSet<GroupPostVideo> GroupPostVideos { get; set; } = null!;
        public virtual DbSet<GroupRole> GroupRoles { get; set; } = null!;
        public virtual DbSet<GroupSetting> GroupSettings { get; set; } = null!;
        public virtual DbSet<GroupSettingUse> GroupSettingUses { get; set; } = null!;
        public virtual DbSet<GroupSharePost> GroupSharePosts { get; set; } = null!;
        public virtual DbSet<GroupStatus> GroupStatuses { get; set; } = null!;
        public virtual DbSet<GroupTag> GroupTags { get; set; } = null!;
        public virtual DbSet<GroupTagUsed> GroupTagUseds { get; set; } = null!;
        public virtual DbSet<GroupType> GroupTypes { get; set; } = null!;
        public virtual DbSet<GroupVideo> GroupVideos { get; set; } = null!;
        public virtual DbSet<Interest> Interests { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<PostReactCount> PostReactCounts { get; set; } = null!;
        public virtual DbSet<ReactComment> ReactComments { get; set; } = null!;
        public virtual DbSet<ReactGroupCommentPost> ReactGroupCommentPosts { get; set; } = null!;
        public virtual DbSet<ReactGroupPhotoPost> ReactGroupPhotoPosts { get; set; } = null!;
        public virtual DbSet<ReactGroupPhotoPostComment> ReactGroupPhotoPostComments { get; set; } = null!;
        public virtual DbSet<ReactGroupPost> ReactGroupPosts { get; set; } = null!;
        public virtual DbSet<ReactGroupSharePost> ReactGroupSharePosts { get; set; } = null!;
        public virtual DbSet<ReactGroupSharePostComment> ReactGroupSharePostComments { get; set; } = null!;
        public virtual DbSet<ReactGroupVideoPost> ReactGroupVideoPosts { get; set; } = null!;
        public virtual DbSet<ReactGroupVideoPostComment> ReactGroupVideoPostComments { get; set; } = null!;
        public virtual DbSet<ReactPhotoPost> ReactPhotoPosts { get; set; } = null!;
        public virtual DbSet<ReactPhotoPostComment> ReactPhotoPostComments { get; set; } = null!;
        public virtual DbSet<ReactPost> ReactPosts { get; set; } = null!;
        public virtual DbSet<ReactSharePost> ReactSharePosts { get; set; } = null!;
        public virtual DbSet<ReactSharePostComment> ReactSharePostComments { get; set; } = null!;
        public virtual DbSet<ReactType> ReactTypes { get; set; } = null!;
        public virtual DbSet<ReactVideoPost> ReactVideoPosts { get; set; } = null!;
        public virtual DbSet<ReactVideoPostComment> ReactVideoPostComments { get; set; } = null!;
        public virtual DbSet<Relationship> Relationships { get; set; } = null!;
        public virtual DbSet<ReportComment> ReportComments { get; set; } = null!;
        public virtual DbSet<ReportPost> ReportPosts { get; set; } = null!;
        public virtual DbSet<ReportProfile> ReportProfiles { get; set; } = null!;
        public virtual DbSet<ReportType> ReportTypes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<SharePost> SharePosts { get; set; } = null!;
        public virtual DbSet<UserChat> UserChats { get; set; } = null!;
        public virtual DbSet<UserClientPermission> UserClientPermissions { get; set; } = null!;
        public virtual DbSet<UserGender> UserGenders { get; set; } = null!;
        public virtual DbSet<UserInterest> UserInterests { get; set; } = null!;
        public virtual DbSet<UserPost> UserPosts { get; set; } = null!;
        public virtual DbSet<UserPostPhoto> UserPostPhotos { get; set; } = null!;
        public virtual DbSet<UserPostVideo> UserPostVideos { get; set; } = null!;
        public virtual DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public virtual DbSet<UserRelationship> UserRelationships { get; set; } = null!;
        public virtual DbSet<UserSetting> UserSettings { get; set; } = null!;
        public virtual DbSet<UserStatus> UserStatuses { get; set; } = null!;
        public virtual DbSet<Video> Videos { get; set; } = null!;
        public virtual DbSet<WebAffiliation> WebAffiliations { get; set; } = null!;
        public virtual DbSet<WorkPlace> WorkPlaces { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("CommandConnection");
                optionsBuilder.UseSqlServer(ConnectionString);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminProfile>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("PK__AdminPro__719FE48897BD1C7C");

                entity.ToTable("AdminProfile");

                entity.Property(e => e.AdminId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AdminProfiles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_role_admin");
            });

            modelBuilder.Entity<AvataPhoto>(entity =>
            {
                entity.HasKey(e => e.AvataPhotosId)
                    .HasName("PK__AvataPho__BF0395A4D152E955");

                entity.ToTable("AvataPhoto");

                entity.Property(e => e.AvataPhotosId).ValueGeneratedNever();

                entity.Property(e => e.AvataPhotosUrl)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AvataPhotos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("avata_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.AvataPhotos)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("status_avata_FK");
            });

            modelBuilder.Entity<BlockType>(entity =>
            {
                entity.ToTable("BlockType");

                entity.Property(e => e.BlockTypeId).ValueGeneratedNever();

                entity.Property(e => e.BlockTypeName).HasMaxLength(100);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<BlockUser>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.UserIsBlockedId })
                    .HasName("PK_Block_UserId");

                entity.ToTable("BlockUser");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.BlockType)
                    .WithMany(p => p.BlockUsers)
                    .HasForeignKey(d => d.BlockTypeId)
                    .HasConstraintName("block_type_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BlockUserUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("block_user_FK");

                entity.HasOne(d => d.UserIsBlocked)
                    .WithMany(p => p.BlockUserUserIsBlockeds)
                    .HasForeignKey(d => d.UserIsBlockedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("is_blocked_user_FK");
            });

            modelBuilder.Entity<ChatAccount>(entity =>
            {
                entity.ToTable("ChatAccount");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChatAccounts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_chat_account");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ClientUrl)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<CommentGroupPost>(entity =>
            {
                entity.ToTable("CommentGroupPost");

                entity.Property(e => e.CommentGroupPostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ListNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.CommentGroupPosts)
                    .HasForeignKey(d => d.GroupPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_post_comment_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentGroupPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_roup_post_user_FK");
            });

            modelBuilder.Entity<CommentGroupSharePost>(entity =>
            {
                entity.ToTable("CommentGroupSharePost");

                entity.Property(e => e.CommentGroupSharePostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ListNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.GroupSharePost)
                    .WithMany(p => p.CommentGroupSharePosts)
                    .HasForeignKey(d => d.GroupSharePostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_coment_group_share_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentGroupSharePosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_coment_group_share_post");
            });

            modelBuilder.Entity<CommentGroupVideoPost>(entity =>
            {
                entity.ToTable("CommentGroupVideoPost");

                entity.Property(e => e.CommentGroupVideoPostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ListNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.GroupPostVideo)
                    .WithMany(p => p.CommentGroupVideoPosts)
                    .HasForeignKey(d => d.GroupPostVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_group_video_comment_p_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentGroupVideoPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_groupp_user_FK");
            });

            modelBuilder.Entity<CommentPhotoGroupPost>(entity =>
            {
                entity.ToTable("CommentPhotoGroupPost");

                entity.Property(e => e.CommentPhotoGroupPostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ListNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.GroupPostPhoto)
                    .WithMany(p => p.CommentPhotoGroupPosts)
                    .HasForeignKey(d => d.GroupPostPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_comment_photo_group_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentPhotoGroupPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_photo_group_user_FK");
            });

            modelBuilder.Entity<CommentPhotoPost>(entity =>
            {
                entity.ToTable("CommentPhotoPost");

                entity.Property(e => e.CommentPhotoPostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ListNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentPhotoPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_photo_post_by_user_FK");

                entity.HasOne(d => d.UserPostPhoto)
                    .WithMany(p => p.CommentPhotoPosts)
                    .HasForeignKey(d => d.UserPostPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_photo_comment_FK");
            });

            modelBuilder.Entity<CommentPost>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK__CommentP__C3B4DFCA0A1B372B");

                entity.ToTable("CommentPost");

                entity.Property(e => e.CommentId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ListNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_post_by_user_FK");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.CommentPosts)
                    .HasForeignKey(d => d.UserPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_comment_FK");
            });

            modelBuilder.Entity<CommentSharePost>(entity =>
            {
                entity.ToTable("CommentSharePost");

                entity.Property(e => e.CommentSharePostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ListNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.SharePost)
                    .WithMany(p => p.CommentSharePosts)
                    .HasForeignKey(d => d.SharePostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_coment_share_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentSharePosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_coment_share_post");
            });

            modelBuilder.Entity<CommentVideoPost>(entity =>
            {
                entity.ToTable("CommentVideoPost");

                entity.Property(e => e.CommentVideoPostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ListNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentVideoPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_user_FK");

                entity.HasOne(d => d.UserPostVideo)
                    .WithMany(p => p.CommentVideoPosts)
                    .HasForeignKey(d => d.UserPostVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_comment_video__FK");
            });

            modelBuilder.Entity<ContactInfo>(entity =>
            {
                entity.ToTable("ContactInfo");

                entity.HasIndex(e => e.UserId, "only_contact")
                    .IsUnique();

                entity.Property(e => e.ContactInfoId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.PrimaryNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.SecondEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SecondNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.ContactInfo)
                    .HasForeignKey<ContactInfo>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contact_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.ContactInfos)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contact_status_FK");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FriendId })
                    .HasName("PK_Friends_UserId");

                entity.ToTable("Friend");

                entity.HasIndex(e => new { e.UserId, e.FriendId }, "UQ_Friends_UserId")
                    .IsUnique();

                entity.Property(e => e.LastInteractionDate).HasColumnType("datetime");

                entity.HasOne(d => d.FriendNavigation)
                    .WithMany(p => p.FriendFriendNavigations)
                    .HasForeignKey(d => d.FriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("friend_senduser_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FriendUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("friendsenduser_FK");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.GenderId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GenderName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupFpt>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__GroupFPT__149AF36A2EEAD8AE");

                entity.ToTable("GroupFPT");

                entity.Property(e => e.GroupId).ValueGeneratedNever();

                entity.Property(e => e.CoverImage)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GroupDescription).HasMaxLength(1000);

                entity.Property(e => e.GroupName).HasMaxLength(200);

                entity.Property(e => e.GroupNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.GroupFpts)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("create_group_FK");

                entity.HasOne(d => d.GroupStatus)
                    .WithMany(p => p.GroupFpts)
                    .HasForeignKey(d => d.GroupStatusId)
                    .HasConstraintName("fk_group_status_group");

                entity.HasOne(d => d.GroupType)
                    .WithMany(p => p.GroupFpts)
                    .HasForeignKey(d => d.GroupTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("type_group_FK");
            });

            modelBuilder.Entity<GroupInvitation>(entity =>
            {
                entity.HasKey(e => e.InvitationId)
                    .HasName("PK__GroupInv__033C8DCF37C14BED");

                entity.ToTable("GroupInvitation");

                entity.Property(e => e.InvitationId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupInvitations)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_invate_FK");

                entity.HasOne(d => d.Invited)
                    .WithMany(p => p.GroupInvitationInviteds)
                    .HasForeignKey(d => d.InvitedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_touser_invated_FK");

                entity.HasOne(d => d.Inviter)
                    .WithMany(p => p.GroupInvitationInviters)
                    .HasForeignKey(d => d.InviterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_user_invate_FK");
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.UserId })
                    .HasName("PK_GroupId_UserId");

                entity.ToTable("GroupMember");

                entity.Property(e => e.JoinedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_memmber_FK");

                entity.HasOne(d => d.GroupRole)
                    .WithMany(p => p.GroupMembers)
                    .HasForeignKey(d => d.GroupRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_role_FK");

                entity.HasOne(d => d.InvatedByNavigation)
                    .WithMany(p => p.GroupMemberInvatedByNavigations)
                    .HasForeignKey(d => d.InvatedBy)
                    .HasConstraintName("fk_invated_group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupMemberUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_user_FK");
            });

            modelBuilder.Entity<GroupPhoto>(entity =>
            {
                entity.ToTable("GroupPhoto");

                entity.Property(e => e.GroupPhotoId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupPhotoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoBigUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoSmallUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPhotos)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("photo_group_FK");
            });

            modelBuilder.Entity<GroupPost>(entity =>
            {
                entity.ToTable("GroupPost");

                entity.Property(e => e.GroupPostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupPostNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("fk_group_group_post");

                entity.HasOne(d => d.GroupPhoto)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.GroupPhotoId)
                    .HasConstraintName("FK_grouppost_photourl");

                entity.HasOne(d => d.GroupStatus)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.GroupStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("gpost_status_FK");

                entity.HasOne(d => d.GroupVideo)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.GroupVideoId)
                    .HasConstraintName("FK_grouppost_videourl");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_post_user_FK");
            });

            modelBuilder.Entity<GroupPostPhoto>(entity =>
            {
                entity.ToTable("GroupPostPhoto");

                entity.Property(e => e.GroupPostPhotoId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupPostPhotoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPostPhotos)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("fk_group_group_post_photo");

                entity.HasOne(d => d.GroupPhoto)
                    .WithMany(p => p.GroupPostPhotos)
                    .HasForeignKey(d => d.GroupPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_post_photo_FK");

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.GroupPostPhotos)
                    .HasForeignKey(d => d.GroupPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_post_ph_FK");

                entity.HasOne(d => d.GroupStatus)
                    .WithMany(p => p.GroupPostPhotos)
                    .HasForeignKey(d => d.GroupStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("gpp_status_FK");
            });

            modelBuilder.Entity<GroupPostReactCount>(entity =>
            {
                entity.ToTable("GroupPostReactCount");

                entity.Property(e => e.GroupPostReactCountId).ValueGeneratedNever();

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.GroupPostReactCounts)
                    .HasForeignKey(d => d.GroupPostId)
                    .HasConstraintName("react_count_group_post");

                entity.HasOne(d => d.GroupPostPhoto)
                    .WithMany(p => p.GroupPostReactCounts)
                    .HasForeignKey(d => d.GroupPostPhotoId)
                    .HasConstraintName("react_count_group_post_photo");

                entity.HasOne(d => d.GroupPostVideo)
                    .WithMany(p => p.GroupPostReactCounts)
                    .HasForeignKey(d => d.GroupPostVideoId)
                    .HasConstraintName("react_count_group_post_group");
            });

            modelBuilder.Entity<GroupPostVideo>(entity =>
            {
                entity.ToTable("GroupPostVideo");

                entity.Property(e => e.GroupPostVideoId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupPostVideoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPostVideos)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("fk_group_group_post_video");

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.GroupPostVideos)
                    .HasForeignKey(d => d.GroupPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_post_video_FK");

                entity.HasOne(d => d.GroupStatus)
                    .WithMany(p => p.GroupPostVideos)
                    .HasForeignKey(d => d.GroupStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_post_videostatus_FK");

                entity.HasOne(d => d.GroupVideo)
                    .WithMany(p => p.GroupPostVideos)
                    .HasForeignKey(d => d.GroupVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_post_video_togroup_FK");
            });

            modelBuilder.Entity<GroupRole>(entity =>
            {
                entity.ToTable("GroupRole");

                entity.Property(e => e.GroupRoleId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupRoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupSetting>(entity =>
            {
                entity.ToTable("GroupSetting");

                entity.Property(e => e.GroupSettingId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupSettingName).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupSettingUse>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.GroupSettingId })
                    .HasName("PK_GroupId_SettingId");

                entity.ToTable("GroupSettingUse");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupSettingUses)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_setting_FK");

                entity.HasOne(d => d.GroupSetting)
                    .WithMany(p => p.GroupSettingUses)
                    .HasForeignKey(d => d.GroupSettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("setting_group_FK");

                entity.HasOne(d => d.GroupStatus)
                    .WithMany(p => p.GroupSettingUses)
                    .HasForeignKey(d => d.GroupStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_setting_status_FK");
            });

            modelBuilder.Entity<GroupSharePost>(entity =>
            {
                entity.ToTable("GroupSharePost");

                entity.Property(e => e.GroupSharePostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupSharePosts)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("fk_group_share_post_group");

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.GroupSharePosts)
                    .HasForeignKey(d => d.GroupPostId)
                    .HasConstraintName("group_share_post_grouppostId_FK");

                entity.HasOne(d => d.GroupPostPhoto)
                    .WithMany(p => p.GroupSharePosts)
                    .HasForeignKey(d => d.GroupPostPhotoId)
                    .HasConstraintName("group_share_post_groupphotopost_FK");

                entity.HasOne(d => d.GroupPostVideo)
                    .WithMany(p => p.GroupSharePosts)
                    .HasForeignKey(d => d.GroupPostVideoId)
                    .HasConstraintName("group_share_post_groupvideopost_FK");

                entity.HasOne(d => d.GroupStatus)
                    .WithMany(p => p.GroupSharePosts)
                    .HasForeignKey(d => d.GroupStatusId)
                    .HasConstraintName("fk_group_share_post_st");

                entity.HasOne(d => d.SharedToUser)
                    .WithMany(p => p.GroupSharePostSharedToUsers)
                    .HasForeignKey(d => d.SharedToUserId)
                    .HasConstraintName("group_share_post_to_user_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupSharePostUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_share_post_by_user_FK");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.GroupSharePosts)
                    .HasForeignKey(d => d.UserPostId)
                    .HasConstraintName("group_share_post_FK");

                entity.HasOne(d => d.UserPostPhoto)
                    .WithMany(p => p.GroupSharePosts)
                    .HasForeignKey(d => d.UserPostPhotoId)
                    .HasConstraintName("group_share_post_postphoto_FK");

                entity.HasOne(d => d.UserPostVideo)
                    .WithMany(p => p.GroupSharePosts)
                    .HasForeignKey(d => d.UserPostVideoId)
                    .HasConstraintName("group_share_post_postvideo_FK");

                entity.HasOne(d => d.UserShared)
                    .WithMany(p => p.GroupSharePostUserShareds)
                    .HasForeignKey(d => d.UserSharedId)
                    .HasConstraintName("fk_group_share_post_user");
            });

            modelBuilder.Entity<GroupStatus>(entity =>
            {
                entity.ToTable("GroupStatus");

                entity.Property(e => e.GroupStatusId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupStatusName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupTag>(entity =>
            {
                entity.HasKey(e => e.TagId)
                    .HasName("PK__GroupTag__657CF9AC5294E2E4");

                entity.ToTable("GroupTag");

                entity.Property(e => e.TagId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.TagName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupTagUsed>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.TagId })
                    .HasName("PK_GroupId_TagId");

                entity.ToTable("GroupTagUsed");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupTagUseds)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_FK");

                entity.HasOne(d => d.GroupStatus)
                    .WithMany(p => p.GroupTagUseds)
                    .HasForeignKey(d => d.GroupStatusId)
                    .HasConstraintName("fk_group_tag_used");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.GroupTagUseds)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tag_group_FK");
            });

            modelBuilder.Entity<GroupType>(entity =>
            {
                entity.ToTable("GroupType");

                entity.Property(e => e.GroupTypeId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupTypeName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<GroupVideo>(entity =>
            {
                entity.ToTable("GroupVideo");

                entity.Property(e => e.GroupVideoId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.GroupVideoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.VideoUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupVideos)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("video_group_FK");
            });

            modelBuilder.Entity<Interest>(entity =>
            {
                entity.ToTable("Interest");

                entity.Property(e => e.InterestId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.InterestName).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.NotiMessage).HasMaxLength(255);

                entity.Property(e => e.NotifiUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("noti_type_FK");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.NotificationSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("noti_senduser_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("noti_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("noti_status_FK");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("NotificationType");

                entity.Property(e => e.NotificationTypeId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.NotificationTypeName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("Photo");

                entity.Property(e => e.PhotoId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.PhotoBigUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoSmallUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("photo_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("photo_status_FK");
            });

            modelBuilder.Entity<PostReactCount>(entity =>
            {
                entity.ToTable("PostReactCount");

                entity.Property(e => e.PostReactCountId).ValueGeneratedNever();

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.PostReactCounts)
                    .HasForeignKey(d => d.UserPostId)
                    .HasConstraintName("react_count_user_post");

                entity.HasOne(d => d.UserPostPhoto)
                    .WithMany(p => p.PostReactCounts)
                    .HasForeignKey(d => d.UserPostPhotoId)
                    .HasConstraintName("react_count_user_post_photo");

                entity.HasOne(d => d.UserPostVideo)
                    .WithMany(p => p.PostReactCounts)
                    .HasForeignKey(d => d.UserPostVideoId)
                    .HasConstraintName("react_count_user_post_group");
            });

            modelBuilder.Entity<ReactComment>(entity =>
            {
                entity.ToTable("ReactComment");

                entity.Property(e => e.ReactCommentId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.ReactComments)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_comment_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactComments)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_comment_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_cmt_user_FK");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.ReactComments)
                    .HasForeignKey(d => d.UserPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_comment_on_post_FK");
            });

            modelBuilder.Entity<ReactGroupCommentPost>(entity =>
            {
                entity.ToTable("ReactGroupCommentPost");

                entity.Property(e => e.ReactGroupCommentPostId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommentGroupPost)
                    .WithMany(p => p.ReactGroupCommentPosts)
                    .HasForeignKey(d => d.CommentGroupPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_comment_group_FK");

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.ReactGroupCommentPosts)
                    .HasForeignKey(d => d.GroupPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_comment_group_post_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactGroupCommentPosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_group_comment_post_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactGroupCommentPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_user_group_FK");
            });

            modelBuilder.Entity<ReactGroupPhotoPost>(entity =>
            {
                entity.ToTable("ReactGroupPhotoPost");

                entity.Property(e => e.ReactGroupPhotoPostId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.GroupPostPhoto)
                    .WithMany(p => p.ReactGroupPhotoPosts)
                    .HasForeignKey(d => d.GroupPostPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_photo_post_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactGroupPhotoPosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_group_photo_post_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactGroupPhotoPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_photo_user_FK");
            });

            modelBuilder.Entity<ReactGroupPhotoPostComment>(entity =>
            {
                entity.HasKey(e => e.ReactPhotoPostCommentId)
                    .HasName("PK__ReactGro__E6B525286DEC3ED0");

                entity.ToTable("ReactGroupPhotoPostComment");

                entity.Property(e => e.ReactPhotoPostCommentId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommentPhotoGroupPost)
                    .WithMany(p => p.ReactGroupPhotoPostComments)
                    .HasForeignKey(d => d.CommentPhotoGroupPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_photo_comment_FK");

                entity.HasOne(d => d.GroupPostPhoto)
                    .WithMany(p => p.ReactGroupPhotoPostComments)
                    .HasForeignKey(d => d.GroupPostPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_photo_comment_post_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactGroupPhotoPostComments)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_group_photo_post_cmt_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactGroupPhotoPostComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_photo_comment_user_FK");
            });

            modelBuilder.Entity<ReactGroupPost>(entity =>
            {
                entity.ToTable("ReactGroupPost");

                entity.Property(e => e.ReactGroupPostId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.ReactGroupPosts)
                    .HasForeignKey(d => d.GroupPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_post_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactGroupPosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_grouppost_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactGroupPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_post_user_FK");
            });

            modelBuilder.Entity<ReactGroupSharePost>(entity =>
            {
                entity.ToTable("ReactGroupSharePost");

                entity.Property(e => e.ReactGroupSharePostId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.GroupSharePost)
                    .WithMany(p => p.ReactGroupSharePosts)
                    .HasForeignKey(d => d.GroupSharePostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_react_group_share_post");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactGroupSharePosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_type_react_group_share_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactGroupSharePosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_react_group_share_post");
            });

            modelBuilder.Entity<ReactGroupSharePostComment>(entity =>
            {
                entity.HasKey(e => e.ReactGroupSharePosCommentId)
                    .HasName("PK__ReactGro__307770EA84AEB094");

                entity.ToTable("ReactGroupSharePostComment");

                entity.Property(e => e.ReactGroupSharePosCommentId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommentGroupSharePost)
                    .WithMany(p => p.ReactGroupSharePostComments)
                    .HasForeignKey(d => d.CommentGroupSharePostId)
                    .HasConstraintName("fk_comment_react_group_share_post_cmt");

                entity.HasOne(d => d.GroupSharePost)
                    .WithMany(p => p.ReactGroupSharePostComments)
                    .HasForeignKey(d => d.GroupSharePostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_react_group_share_post_cmt");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactGroupSharePostComments)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_type_react_group_share_post_cmt");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactGroupSharePostComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_react_group_share_post_cmt");
            });

            modelBuilder.Entity<ReactGroupVideoPost>(entity =>
            {
                entity.ToTable("ReactGroupVideoPost");

                entity.Property(e => e.ReactGroupVideoPostId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.GroupPostVideo)
                    .WithMany(p => p.ReactGroupVideoPosts)
                    .HasForeignKey(d => d.GroupPostVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_video_post_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactGroupVideoPosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_group_video_post_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactGroupVideoPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_video_user_FK");
            });

            modelBuilder.Entity<ReactGroupVideoPostComment>(entity =>
            {
                entity.HasKey(e => e.ReactGroupVideoCommentId)
                    .HasName("PK__ReactGro__B37F79E410C4D8CF");

                entity.ToTable("ReactGroupVideoPostComment");

                entity.Property(e => e.ReactGroupVideoCommentId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommentGroupVideoPost)
                    .WithMany(p => p.ReactGroupVideoPostComments)
                    .HasForeignKey(d => d.CommentGroupVideoPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_video_comment_FK");

                entity.HasOne(d => d.GroupPostVideo)
                    .WithMany(p => p.ReactGroupVideoPostComments)
                    .HasForeignKey(d => d.GroupPostVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_video_comment_post_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactGroupVideoPostComments)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_group_video_post_comment_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactGroupVideoPostComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_group_video_comment_user_FK");
            });

            modelBuilder.Entity<ReactPhotoPost>(entity =>
            {
                entity.ToTable("ReactPhotoPost");

                entity.Property(e => e.ReactPhotoPostId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactPhotoPosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_photo_post_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactPhotoPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_photo_post_by_user_FK");

                entity.HasOne(d => d.UserPostPhoto)
                    .WithMany(p => p.ReactPhotoPosts)
                    .HasForeignKey(d => d.UserPostPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_photo_on_post_FK");
            });

            modelBuilder.Entity<ReactPhotoPostComment>(entity =>
            {
                entity.ToTable("ReactPhotoPostComment");

                entity.Property(e => e.ReactPhotoPostCommentId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommentPhotoPost)
                    .WithMany(p => p.ReactPhotoPostComments)
                    .HasForeignKey(d => d.CommentPhotoPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_pt_comment_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactPhotoPostComments)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_photopost_comment_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactPhotoPostComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_photo_comment_by_user_FK");

                entity.HasOne(d => d.UserPostPhoto)
                    .WithMany(p => p.ReactPhotoPostComments)
                    .HasForeignKey(d => d.UserPostPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_photo_comment_post_FK");
            });

            modelBuilder.Entity<ReactPost>(entity =>
            {
                entity.ToTable("ReactPost");

                entity.Property(e => e.ReactPostId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactPosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_post_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_post_user_FK");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.ReactPosts)
                    .HasForeignKey(d => d.UserPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_post_FK");
            });

            modelBuilder.Entity<ReactSharePost>(entity =>
            {
                entity.ToTable("ReactSharePost");

                entity.Property(e => e.ReactSharePostId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactSharePosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_type_react_share_post");

                entity.HasOne(d => d.SharePost)
                    .WithMany(p => p.ReactSharePosts)
                    .HasForeignKey(d => d.SharePostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_react_share_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactSharePosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_react_share_post");
            });

            modelBuilder.Entity<ReactSharePostComment>(entity =>
            {
                entity.HasKey(e => e.ReactSharePosCommentId)
                    .HasName("PK__ReactSha__F43E716EA5D4E3F8");

                entity.ToTable("ReactSharePostComment");

                entity.Property(e => e.ReactSharePosCommentId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommentSharePost)
                    .WithMany(p => p.ReactSharePostComments)
                    .HasForeignKey(d => d.CommentSharePostId)
                    .HasConstraintName("fk_comment_react_share_post_cmt");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactSharePostComments)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_type_react_share_post_cmt");

                entity.HasOne(d => d.SharePost)
                    .WithMany(p => p.ReactSharePostComments)
                    .HasForeignKey(d => d.SharePostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_react_share_post_cmt");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactSharePostComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_react_share_post_cmt");
            });

            modelBuilder.Entity<ReactType>(entity =>
            {
                entity.ToTable("ReactType");

                entity.Property(e => e.ReactTypeId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ReactTypeName).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReactVideoPost>(entity =>
            {
                entity.ToTable("ReactVideoPost");

                entity.Property(e => e.ReactVideoPostId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactVideoPosts)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_video_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactVideoPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_user_FK");

                entity.HasOne(d => d.UserPostVideo)
                    .WithMany(p => p.ReactVideoPosts)
                    .HasForeignKey(d => d.UserPostVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_video_on_post_FK");
            });

            modelBuilder.Entity<ReactVideoPostComment>(entity =>
            {
                entity.ToTable("ReactVideoPostComment");

                entity.Property(e => e.ReactVideoPostCommentId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommentVideoPost)
                    .WithMany(p => p.ReactVideoPostComments)
                    .HasForeignKey(d => d.CommentVideoPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_vid_comment_FK");

                entity.HasOne(d => d.ReactType)
                    .WithMany(p => p.ReactVideoPostComments)
                    .HasForeignKey(d => d.ReactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reacttype_videopost_comment_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReactVideoPostComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_vidcmt_user_FK");

                entity.HasOne(d => d.UserPostVideo)
                    .WithMany(p => p.ReactVideoPostComments)
                    .HasForeignKey(d => d.UserPostVideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("react_video_comment_on_post_FK");
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.ToTable("Relationship");

                entity.Property(e => e.RelationshipId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.RelationshipName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReportComment>(entity =>
            {
                entity.ToTable("ReportComment");

                entity.Property(e => e.ReportCommentId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CommentGroupPost)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.CommentGroupPostId)
                    .HasConstraintName("report_comment__group_post_FK");

                entity.HasOne(d => d.CommentGroupSharePost)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.CommentGroupSharePostId)
                    .HasConstraintName("fk_report_comment_group_share_post");

                entity.HasOne(d => d.CommentGroupVideoPost)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.CommentGroupVideoPostId)
                    .HasConstraintName("report_comment_group_video_post_FK");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("report_comment_post_FK");

                entity.HasOne(d => d.CommentPhotoGroupPost)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.CommentPhotoGroupPostId)
                    .HasConstraintName("report_comment_group_photo_post_FK");

                entity.HasOne(d => d.CommentPhotoPost)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.CommentPhotoPostId)
                    .HasConstraintName("report_comment_photo_post_FK");

                entity.HasOne(d => d.CommentSharePost)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.CommentSharePostId)
                    .HasConstraintName("fk_report_comment_share_post");

                entity.HasOne(d => d.CommentVideoPost)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.CommentVideoPostId)
                    .HasConstraintName("report_comment_video_post_FK");

                entity.HasOne(d => d.ReportBy)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.ReportById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_comment_by_FK");

                entity.HasOne(d => d.ReportType)
                    .WithMany(p => p.ReportComments)
                    .HasForeignKey(d => d.ReportTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_comment_type_FK");
            });

            modelBuilder.Entity<ReportPost>(entity =>
            {
                entity.ToTable("ReportPost");

                entity.Property(e => e.ReportPostId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.GroupPostId)
                    .HasConstraintName("report_group_post_FK");

                entity.HasOne(d => d.GroupPostPhoto)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.GroupPostPhotoId)
                    .HasConstraintName("fk_report_group_photo_post");

                entity.HasOne(d => d.GroupPostVideo)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.GroupPostVideoId)
                    .HasConstraintName("fk_report_group_video_post");

                entity.HasOne(d => d.GroupSharePost)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.GroupSharePostId)
                    .HasConstraintName("fk_report_group_share_post");

                entity.HasOne(d => d.ReportBy)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.ReportById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_post_by_FK");

                entity.HasOne(d => d.ReportType)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.ReportTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_post_type_FK");

                entity.HasOne(d => d.SharePost)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.SharePostId)
                    .HasConstraintName("fk_report_share_post");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.UserPostId)
                    .HasConstraintName("report_user_post_FK");

                entity.HasOne(d => d.UserPostPhoto)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.UserPostPhotoId)
                    .HasConstraintName("fk_report_photo_post");

                entity.HasOne(d => d.UserPostVideo)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.UserPostVideoId)
                    .HasConstraintName("fk_report_video_post");
            });

            modelBuilder.Entity<ReportProfile>(entity =>
            {
                entity.ToTable("ReportProfile");

                entity.Property(e => e.ReportProfileId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ReportProfiles)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("report_group_FK");

                entity.HasOne(d => d.ReportBy)
                    .WithMany(p => p.ReportProfileReportBies)
                    .HasForeignKey(d => d.ReportById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_profile_by_FK");

                entity.HasOne(d => d.ReportType)
                    .WithMany(p => p.ReportProfiles)
                    .HasForeignKey(d => d.ReportTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_profile_type_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReportProfileUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("report_user_FK");
            });

            modelBuilder.Entity<ReportType>(entity =>
            {
                entity.ToTable("ReportType");

                entity.Property(e => e.ReportTypeId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ReportTypeName).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.NameRole)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

                entity.Property(e => e.SettingId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.SettingName).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<SharePost>(entity =>
            {
                entity.ToTable("SharePost");

                entity.Property(e => e.SharePostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.GroupPost)
                    .WithMany(p => p.SharePosts)
                    .HasForeignKey(d => d.GroupPostId)
                    .HasConstraintName("share_post_grouppostId_FK");

                entity.HasOne(d => d.GroupPostPhoto)
                    .WithMany(p => p.SharePosts)
                    .HasForeignKey(d => d.GroupPostPhotoId)
                    .HasConstraintName("share_post_groupphotopost_FK");

                entity.HasOne(d => d.GroupPostVideo)
                    .WithMany(p => p.SharePosts)
                    .HasForeignKey(d => d.GroupPostVideoId)
                    .HasConstraintName("share_post_groupvideopost_FK");

                entity.HasOne(d => d.SharedToUser)
                    .WithMany(p => p.SharePostSharedToUsers)
                    .HasForeignKey(d => d.SharedToUserId)
                    .HasConstraintName("share_post_to_user_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SharePostUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("share_post_by_user_FK");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.SharePosts)
                    .HasForeignKey(d => d.UserPostId)
                    .HasConstraintName("share_post_FK");

                entity.HasOne(d => d.UserPostPhoto)
                    .WithMany(p => p.SharePosts)
                    .HasForeignKey(d => d.UserPostPhotoId)
                    .HasConstraintName("share_post_postphoto_FK");

                entity.HasOne(d => d.UserPostVideo)
                    .WithMany(p => p.SharePosts)
                    .HasForeignKey(d => d.UserPostVideoId)
                    .HasConstraintName("share_post_postvideo_FK");

                entity.HasOne(d => d.UserShared)
                    .WithMany(p => p.SharePostUserShareds)
                    .HasForeignKey(d => d.UserSharedId)
                    .HasConstraintName("fk_user_share_post_user");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.SharePosts)
                    .HasForeignKey(d => d.UserStatusId)
                    .HasConstraintName("fk_share_post_status");
            });

            modelBuilder.Entity<UserChat>(entity =>
            {
                entity.ToTable("UserChat");

                entity.Property(e => e.UserChatId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.ChatWith)
                    .WithMany(p => p.UserChatChatWiths)
                    .HasForeignKey(d => d.ChatWithId)
                    .HasConstraintName("fk_chat_with_user");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserChatUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_chat");
            });

            modelBuilder.Entity<UserClientPermission>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UserClientPermission");

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Admin)
                    .WithMany()
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("fk_admin_client");

                entity.HasOne(d => d.Client)
                    .WithMany()
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_client_permission");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_permission");
            });

            modelBuilder.Entity<UserGender>(entity =>
            {
                entity.ToTable("UserGender");

                entity.HasIndex(e => e.UserId, "only_gender")
                    .IsUnique();

                entity.Property(e => e.UserGenderId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.UserGenders)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Gender_rlf_FK");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserGender)
                    .HasForeignKey<UserGender>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Gender_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.UserGenders)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Gender_status_FK");
            });

            modelBuilder.Entity<UserInterest>(entity =>
            {
                entity.ToTable("UserInterest");

                entity.Property(e => e.UserInterestId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Interest)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.InterestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Interest_interest_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Interest_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Interest_status_FK");
            });

            modelBuilder.Entity<UserPost>(entity =>
            {
                entity.ToTable("UserPost");

                entity.Property(e => e.UserPostId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserPostNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.UserPosts)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_userpost_photourl");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.UserPosts)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("up_status_FK");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.UserPosts)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("FK_userpost_videourl");
            });

            modelBuilder.Entity<UserPostPhoto>(entity =>
            {
                entity.ToTable("UserPostPhoto");

                entity.Property(e => e.UserPostPhotoId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserPostPhotoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.UserPostPhotos)
                    .HasForeignKey(d => d.PhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_photo_FK");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.UserPostPhotos)
                    .HasForeignKey(d => d.UserPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("photo_post_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.UserPostPhotos)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("upp_tatus_FK");
            });

            modelBuilder.Entity<UserPostVideo>(entity =>
            {
                entity.ToTable("UserPostVideo");

                entity.Property(e => e.UserPostVideoId).ValueGeneratedNever();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserPostVideoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.UserPostVideos)
                    .HasForeignKey(d => d.UserPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("video_post_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.UserPostVideos)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("upv_status_FK");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.UserPostVideos)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_video_FK");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserProf__1788CC4CF185CD9E");

                entity.ToTable("UserProfile");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.AboutMe).HasMaxLength(1000);

                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.Campus).HasMaxLength(100);

                entity.Property(e => e.CoverImage)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FeId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.HomeTown).HasMaxLength(500);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("role_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profile_status_FK");

                entity.HasMany(d => d.Followeds)
                    .WithMany(p => p.Followers)
                    .UsingEntity<Dictionary<string, object>>(
                        "Follower",
                        l => l.HasOne<UserProfile>().WithMany().HasForeignKey("FollowedId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("follow_senduser_FK"),
                        r => r.HasOne<UserProfile>().WithMany().HasForeignKey("FollowerId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("follow_user_FK"),
                        j =>
                        {
                            j.HasKey("FollowerId", "FollowedId").HasName("PK_Followers");

                            j.ToTable("Follower");
                        });

                entity.HasMany(d => d.Followers)
                    .WithMany(p => p.Followeds)
                    .UsingEntity<Dictionary<string, object>>(
                        "Follower",
                        l => l.HasOne<UserProfile>().WithMany().HasForeignKey("FollowerId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("follow_user_FK"),
                        r => r.HasOne<UserProfile>().WithMany().HasForeignKey("FollowedId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("follow_senduser_FK"),
                        j =>
                        {
                            j.HasKey("FollowerId", "FollowedId").HasName("PK_Followers");

                            j.ToTable("Follower");
                        });
            });

            modelBuilder.Entity<UserRelationship>(entity =>
            {
                entity.ToTable("UserRelationship");

                entity.HasIndex(e => new { e.UserId, e.RelationshipId }, "UX_UserId_RelationshipId")
                    .IsUnique();

                entity.Property(e => e.UserRelationshipId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Relationship)
                    .WithMany(p => p.UserRelationships)
                    .HasForeignKey(d => d.RelationshipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rlu_rlf_FK");

                entity.HasOne(d => d.User)
                     .WithOne(p => p.UserRelationship)
                     .HasForeignKey<UserRelationship>(d => d.UserId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("rlu_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.UserRelationships)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rlu_status_FK");
            });

            modelBuilder.Entity<UserSetting>(entity =>
            {
                entity.ToTable("UserSetting");

                entity.Property(e => e.UserSettingId).ValueGeneratedNever();

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.UserSettings)
                    .HasForeignKey(d => d.SettingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("setting_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSettings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserSetti__UserI__2D7CBDC4");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.UserSettings)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usetting_status_FK");
            });

            modelBuilder.Entity<UserStatus>(entity =>
            {
                entity.ToTable("UserStatus");

                entity.Property(e => e.UserStatusId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.ToTable("Video");

                entity.Property(e => e.VideoId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.VideoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VideoUrl)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Videos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("video_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Videos)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("video_status_FK");
            });

            modelBuilder.Entity<WebAffiliation>(entity =>
            {
                entity.ToTable("WebAffiliation");

                entity.Property(e => e.WebAffiliationId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.WebAffiliationUrl)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WebAffiliations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("web_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.WebAffiliations)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("web_status_FK");
            });

            modelBuilder.Entity<WorkPlace>(entity =>
            {
                entity.ToTable("WorkPlace");

                entity.Property(e => e.WorkPlaceId).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.WorkPlaceName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkPlaces)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("workpalce_user_FK");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.WorkPlaces)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("wp_status_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
