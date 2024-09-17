using API.Controllers;
using Application.Commands.CreateContactInfor;
using Application.Commands.CreateGender;
using Application.Commands.CreateInterest;
using Application.Commands.CreateRelationships;
using Application.Commands.CreateRole;
using Application.Commands.CreateSettings;
using Application.Commands.CreateStatus;
using Application.Commands.CreateUserGender;
using Application.Commands.CreateUserInterest;
using Application.Commands.GetUserProfile;
using Application.DTO.CreateUserDTO;
using Application.Queries.GenInterest;
using Application.Queries.GetGender;
using Application.Queries.GetUserByUserId;
using Application.Queries.GetUserProfile;
using Application.Queries.GetWebAffilication;
using AutoMapper;
using Command = Domain.CommandModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Query = Domain.QueryModels;
using Application.DTO.GetUserProfileDTO;
using Application.Queries.GetOtherUser;
using Application.Commands.UpdateUserCommand;
using Application.Queries.GetUserStatus;
using Application.Queries.GetUserPost;
using Application.Commands.AddFriendCommand;
using Application.Queries.GetRelationship;
using Application.Queries.GetUserRelationships;
using Application.Queries.GetSettings;
using Application.Commands.UpdateUserSettings;
using Application.DTO.UpdateSettingDTO;
using Application.Commands.CreateReportType;
using Application.Commands.Post;
using Application.Commands.CreateGroupRole;
using Application.Queries.GetAllGroupRole;
using Application.Commands.CreateGroupTag;
using Application.Queries.GetAllGroupTag;
using Application.Commands.CreateGroupSetting;
using Application.Queries.GetAllGroupSetting;
using Application.Commands.CreateUserCommentPost;
using Application.Queries.GetCommentByPostId;
using Application.DTO.CommentDTO;
using Application.Commands.CreateGroupType;
using Application.Queries.GetAllGroupType;
using Application.Commands.CreateGroupStatus;
using Application.Queries.GetAllGroupStatus;
using Application.Commands.CreateGroupCommand;
using Application.Commands.CreateUserCommentVideoPost;
using Application.Commands.CreateUserCommentPhotoPost;
using Application.Queries.GetPost;
using Domain.QueryModels;
using Application.DTO.UserPostDTO;
using Application.DTO.UserPostPhotoDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Commands.CreateReactUserPost;
using Application.Commands.CreateNewReact;
using Application.Commands.CreateReactUserPhotoPost;
using Application.Commands.CreateReactUserVideoPost;
using Application.Commands.CreateGroupPost;
using Application.Queries.GetUserNotificationsList;
using Application.Commands.UpdateNotificationStatus;
using Application.Commands.UpdateUserPostCommand;
using Application.Queries.GetAllReactType;
using Application.Commands.CreateUserCommentGroupPost;
using Application.Commands.CreateUserCommentGroupPhotoPost;
using Application.Commands.CreateUserCommentGroupVideoPost;
using Application.Commands.CreateReactCommentUserPost;
using Application.Commands.CreateReactCommentUserPostVideo;
using Application.Queries.GetGroupStatusForCreate;
using Application.DTO.GroupPostDTO;
using Application.DTO.GroupFPTDTO;
using Application.Commands.ShareUserPostCommand;
using Application.Commands.UpdateUserPhotoPost;
using Application.Commands.UpdateUserVideoPost;
using Application.Commands.UpdateCommentUserPost;
using Application.Commands.UpdateCommentUserPhotoPost;
using Application.Commands.UpdateCommentUserVideoPost;
using Application.DTO.GroupDTO;
using Application.Commands.CreateReactGroupPost;
using Application.Commands.CreateReactGroupVideoPost;
using Application.Commands.CreateReactGroupPhotoPost;
using Application.Commands.CreateReactCommentGroupPostPhoto;
using Application.Commands.CreateReactCommentGroupPost;
using Application.Commands.CreateReactCommentGroupVideoPost;
using Application.Commands.UpdateGroupPostCommand;
using Application.Commands.UpdateGroupPhotoPostCommand;
using Application.Commands.UpdateGroupVideoPostCommand;
using Application.Commands.CreateReportComment;
using Application.Commands.ShareGroupPostCommand;
using Application.Commands.CreateReactForSharePost;
using Application.Commands.CreateReactForGroupSharePost;
using Application.Commands.CreateReactForCommentSharePost;
using Application.Commands.CreateReactForCommentGroupSharePost;
using Application.Commands.UpdateCommentSharePost;
using Application.Commands.UpdateCommentGroupSharePost;
using Application.Queries.GetCommentBySharePost;
using Application.Queries.GetCommentByGroupSharePost;
using Application.Queries.GetReactBySharePostId;
using Application.Queries.GetReactByGroupSharePostId;
using Application.Commands.CreateCommentForSharePost;
using Application.Commands.UpdateSharePost;
using Application.Commands.CreateCommentForGroupSharePost;
using Application.Commands.UpdateGroupSharePost;
using Application.Commands.UpdateCommentGroupPost;
using Application.Commands.UpdateCommentGroupVideoPost;
using Application.Commands.UpdateCommentGroupPhotoPost;
using Application.Commands.CreateReportPost;
using Application.Commands.CreateReportProfile;
using Application.DTO.NotificationDTO;

namespace Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserProfileCommand, Command.UserProfile>()
                .ForMember(dist => dist.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dist => dist.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dist => dist.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dist => dist.FeId, opt => opt.MapFrom(src => src.FeId))
                .ForMember(dist => dist.BirthDay, opt => opt.MapFrom(src => src.BirthDay))
                .ForMember(dist => dist.AboutMe, opt => opt.MapFrom(src => src.AboutMe))
                .ForMember(dist => dist.HomeTown, opt => opt.MapFrom(src => src.HomeTown))
                .ForMember(dist => dist.CoverImage, opt => opt.MapFrom(src => src.CoverImage))
                .ForMember(dist => dist.UserNumber, opt => opt.MapFrom(src => src.UserNumber));
            CreateMap<Query.UserProfile, GetUserQueryResult>()
              .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.NameRole))
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.UserStatus.UserStatusId))
            .ForMember(dest => dest.UserStatusName, opt => opt.MapFrom(src => src.UserStatus.StatusName))
            .ForMember(dest => dest.GenderId, opt => opt.MapFrom(src => src.UserGender.Gender.GenderId))
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.UserGender.Gender.GenderName))
            .ForMember(dest => dest.Relationship, opt => opt.MapFrom(src => src.UserRelationship.Relationship.RelationshipName))
            .ForMember(dest => dest.WebAffiliationUrl, opt => opt.MapFrom(src => src.WebAffiliations))
            .ForMember(dest => dest.AvataPhotosUrl, opt => opt.MapFrom(src => src.AvataPhotos));

            CreateMap<Query.Gender, GetGenderReuslt>();
            CreateMap<UserProfileCommand, UserProfileCommandResult>()
           .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Ignore properties that are not in source
           .ForMember(dest => dest.RoleId, opt => opt.Ignore())
           .ForMember(dest => dest.IsFirstTimeLogin, opt => opt.Ignore())
           .ForMember(dest => dest.UserStatusId, opt => opt.Ignore())
           .ForMember(dest => dest.IsActive, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<Query.Interest, GetInterestResult>();
            CreateMap<Command.UserInterest, UserInterestCommandResult>();
            CreateMap<Query.WebAffiliation, GetWebAffilicationResult>()
                .ForMember(dist => dist.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));

            CreateMap<Query.UserProfile, GetUserByUserIdResult>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.NameRole))
            .ForMember(dest => dest.UserStatusName, opt => opt.MapFrom(src => src.UserStatus.StatusName));


            CreateMap<Query.UserProfile, GetOtherUserQueryResult>();
            CreateMap<Query.Setting, GetSettingsQueryResult>();
            CreateMap<UpdateUserCommand, Query.UserProfile>();
            CreateMap<Query.UserPost, GetUserPostResult>();
            CreateMap<Query.UserPost, UpdateUserPostCommandResult>();
            CreateMap<UserPostPhoto, UserPostPhotoDTO>().ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();
            CreateMap<UserPostVideo, UserPostVideoDTO>().ReverseMap();
            CreateMap<Command.Video, VideoDTO>().ReverseMap();
            CreateMap<Query.Video, VideoDTO>().ReverseMap();

            // Định nghĩa ánh xạ cho Domain.CommandModels.Photo
            CreateMap<Domain.CommandModels.Photo, PhotoDTO>();

            // Định nghĩa ánh xạ cho Photo nếu cần
            CreateMap<Query.Photo, PhotoDTO>();
            CreateMap<Command.Photo, PhotoDTO>();
            CreateMap<AvataPhoto, GetUserAvatar>();
            CreateMap<Domain.CommandModels.AvataPhoto, GetUserAvatar>();



            CreateMap<Command.Gender, CreateGenderCommandResult>();
            CreateMap<Command.ContactInfo, CreateContactInforCommandResult>();
            CreateMap<Command.UserStatus, CreateStatusCommandResult>();
            CreateMap<Command.Relationship, CreateRelationShipCommandResult>();
            CreateMap<Command.Setting, CreateSettingsCommandResult>();
            CreateMap<Command.Role, CreateRoleCommandResult>();
            CreateMap<Command.Interest, CreateInterestCommandResult>();
            CreateMap<CreateUserGenderCommand, Command.UserGender>();
            CreateMap<Command.UserGender, CreateUserGenderCommandResult>();

            //Report
            CreateMap<Command.ReportType, CreateReportTypeCommandResult>();
            CreateMap<Command.ReportComment, CreateReportCommentCommandResult>();
            CreateMap<Command.ReportPost, CreateReportPostCommandResult>();
            CreateMap<Command.ReportProfile, CreateReportProfileCommandResult>();


            //Mapping for getuser
            CreateMap<Query.WorkPlace, GetUserWorkPlaceDTO>();
            CreateMap<Query.UserInterest, GetUserInterers>()
                .ForMember(dest => dest.InteresName, opt => opt.MapFrom(src => src.Interest.InterestName))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.UserStatus.StatusName));
            CreateMap<Query.ContactInfo, GetUserContactInfo>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.UserStatus.StatusName));
            CreateMap<Query.UserGender, GetUserGenderDTO>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.UserStatus.StatusName))
                .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender.GenderName));
            CreateMap<Query.UserRelationship, GetUserWorkPlaceDTO>();
            CreateMap<Query.AvataPhoto, GetUserAvatar>()
               .ForMember(dest => dest.StatusName, otp => otp.MapFrom(src => src.UserStatus.StatusName));
            CreateMap<Query.UserRelationship, GetUserRelationship>()
                .ForMember(dest => dest.StatusName, otp => otp.MapFrom(src => src.UserStatus.StatusName));
            CreateMap<Query.WebAffiliation, GetUserWebAfflication>();

            //update user

            CreateMap<UpdateUserCommand, UpdateUserCommandResult>();
            CreateMap<Query.AvataPhoto, Command.AvataPhoto>();
            CreateMap<Query.UserGender, Command.UserGender>();
            CreateMap<Command.UserGender, Query.UserGender>();
            CreateMap<Command.UserProfile, Query.UserProfile>();
            CreateMap<Query.UserProfile, Command.UserProfile>();
            CreateMap<Command.UserStatus, Query.UserStatus>();
            CreateMap<Query.UserStatus, Command.UserStatus>();
            CreateMap<Query.Gender, Command.Gender>().ReverseMap();
            CreateMap<Query.ContactInfo, Command.ContactInfo>().ReverseMap();
            CreateMap<Query.Relationship, Command.Relationship>().ReverseMap();
            CreateMap<Query.WebAffiliation, Command.WebAffiliation>().ReverseMap();
            CreateMap<Query.Interest, Command.Interest>().ReverseMap();
            CreateMap<Query.WorkPlace, Command.WorkPlace>().ReverseMap();


            CreateMap<Query.UserStatus, GetUserStatusQueryResult>();
            CreateMap<UpdateUserSettingsCommand, UpdateUserCommandResult>().ReverseMap();

            //Add Friend:
            CreateMap<Command.Friend, AddFriendCommandResult>()
                .ForMember(dest => dest.SendBy, otp => otp.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.ReceiptBy, otp => otp.MapFrom(src => src.FriendNavigation.FirstName + " " + src.FriendNavigation.LastName));

            CreateMap<Query.Relationship, GetRelationshipResult>();
            CreateMap<Query.UserRelationship, GetUserRelationshipResult>()
                .ForMember(dest => dest.RelationshipName, otp => otp.MapFrom(src => src.Relationship.RelationshipName));
            CreateMap<Query.UserSetting, UserSettingDTO>();

            //Group
            CreateMap<Command.GroupRole, CreateGroupRoleCommandResult>();
            CreateMap<Command.GroupTag, CreateGroupTagCommandResult>();
            CreateMap<Query.GroupRole, GetAllGroupRoleQueryResult>();
            CreateMap<Command.GroupTag, CreateGroupTagCommandResult>();
            CreateMap<Query.GroupTag, GetAllGroupTagQueryResult>();
            CreateMap<Command.GroupSetting, CreateGroupSettingCommandResult>();
            CreateMap<Query.GroupSetting, GetAllGroupSettingQueryResult>();
            CreateMap<Command.UserPost, CreateUserPostCommandResult>()
               .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Command.ReactGroupPost, CreateReactGroupPostCommandResult>();
            CreateMap<Command.ReactGroupVideoPost, CreateReactGroupVideoPostCommandResult>();
            CreateMap<Command.ReactGroupPhotoPost, CreateReactGroupPhotoPostCommandResult>();
            CreateMap<Command.ReactGroupVideoPostComment, CreateReactCommentGroupPostVideoCommandResult>();
            CreateMap<Command.ReactGroupPhotoPostComment, CreateReactCommentGroupPostPhotoCommandResult>();
            CreateMap<Command.ReactGroupCommentPost, CreateReactCommentGroupPostCommandResult>();

            CreateMap<Query.ReactGroupPost, CreateReactGroupPostCommandResult>();
            CreateMap<Query.ReactGroupVideoPost, CreateReactGroupVideoPostCommandResult>();
            CreateMap<Query.ReactGroupPhotoPost, CreateReactGroupPhotoPostCommandResult>();
            CreateMap<Query.ReactGroupVideoPostComment, CreateReactCommentGroupPostVideoCommandResult>();
            CreateMap<Query.ReactGroupPhotoPostComment, CreateReactCommentGroupPostPhotoCommandResult>();
            CreateMap<Query.ReactGroupCommentPost, CreateReactCommentGroupPostCommandResult>();


            CreateMap<Command.CommentPost, CreateUserCommentPostCommandResult>()
                .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            
            CreateMap<Command.CommentPhotoPost, CreateUserCommentPhotoPostCommandResult>()
                .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Command.UserPost, UpdateUserPostCommandResult>()
               .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Command.UserPostPhoto, UpdateUserPhotoPostCommandResult>()
               .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Command.UserPostVideo, UpdateUserVideoPostCommandResult>()
               .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Query.UserPost, UpdateUserPostCommandResult>()
               .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Query.UserPostPhoto, UpdateUserPhotoPostCommandResult>()
               .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Query.UserPostVideo, UpdateUserVideoPostCommandResult>()
               .ForMember(dest => dest.BannedWords, opt => opt.Ignore());

            CreateMap<Query.CommentPost, GetCommentByPostIdQueryResult>();
            CreateMap<Query.CommentPost, CommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
            CreateMap<Command.CommentVideoPost, CreateUserCommentVideoPostCommandResult>()
                .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Command.GroupType, CreateGroupTypeCommandResult>();
            CreateMap<Query.GroupType, GetAllGroupTypeQueryResult>();
            CreateMap<Command.GroupStatus, CreateGroupStatusCommandResult>();
            CreateMap<Query.GroupStatus, GetGroupStatusQueryResult>();
            CreateMap<Command.GroupFpt, CreateGroupCommandResult>();
            
            CreateMap<Command.GroupPost, CreateGroupPostCommandResult>();
            CreateMap<Command.GroupPost, UpdateGroupPostCommandResult>();
            CreateMap<Command.GroupPostVideo, UpdateGroupVideoPostCommandResult>();
            CreateMap<Command.GroupPostPhoto, UpdateGroupPhotoPostCommandResult>();
            CreateMap<Query.GroupPost, UpdateGroupPostCommandResult>();
            CreateMap<Query.GroupPostVideo, UpdateGroupVideoPostCommandResult>();
            CreateMap<Query.GroupPostPhoto, UpdateGroupPhotoPostCommandResult>();

            CreateMap<UserPost, UserPostDTO>()
            .ForMember(dest => dest.UserPostId, opt => opt.MapFrom(src => src.UserPostId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.UserPostNumber, opt => opt.MapFrom(src => src.UserPostNumber))
            .ForMember(dest => dest.UserStatusId, opt => opt.MapFrom(src => src.UserStatusId))
            .ForMember(dest => dest.IsAvataPost, opt => opt.MapFrom(src => src.IsAvataPost))
            .ForMember(dest => dest.IsCoverPhotoPost, opt => opt.MapFrom(src => src.IsCoverPhotoPost))
            .ForMember(dest => dest.IsHide, opt => opt.MapFrom(src => src.IsHide))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.PhotoId))
            .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.VideoId))
            .ForMember(dest => dest.NumberPost, opt => opt.MapFrom(src => src.NumberPost))
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
            .ForMember(dest => dest.Video, opt => opt.MapFrom(src => src.Video))
            .ForMember(dest => dest.UserPostPhotos, opt => opt.MapFrom(src => src.UserPostPhotos))
            .ForMember(dest => dest.UserPostVideos, opt => opt.MapFrom(src => src.UserPostVideos))
            .ReverseMap();
            CreateMap<UserPostDTO, GetPostResult>();

            CreateMap<Command.CommentGroupPost, CreateUserCommentGroupPostCommandResult>()
            .ForMember(dest => dest.BannedWords, opt => opt.Ignore());

            CreateMap<Command.CommentPhotoGroupPost, CreateUserCommentGroupPhotoPostCommandResult>()
            .ForMember(dest => dest.BannedWords, opt => opt.Ignore());

            CreateMap<Command.CommentGroupVideoPost, CreateUserCommentGroupVideoPostCommandResult>()
            .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Query.GroupStatus, GetGroupStatusForCreateQueryResult>();

            CreateMap<Command.SharePost, ShareUserPostCommandResult>();
            CreateMap<Command.CommentPost, UpdateCommentUserPostCommandResult>()
            .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Command.CommentPhotoPost, UpdateCommentUserPhotoPostCommandResult>()
                .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Command.CommentVideoPost, UpdateCommentUserVideoPostCommandResult>()
                .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Query.CommentPost, UpdateCommentUserPostCommandResult>()
            .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Query.CommentPhotoPost, UpdateCommentUserPhotoPostCommandResult>()
                .ForMember(dest => dest.BannedWords, opt => opt.Ignore());
            CreateMap<Query.CommentVideoPost, UpdateCommentUserVideoPostCommandResult>()
                .ForMember(dest => dest.BannedWords, opt => opt.Ignore());

            //user react
            CreateMap<Command.ReactPost, CreateReactUserPostCommandResult>();
            CreateMap<Command.ReactType, CreateNewReactCommandResult>();
            CreateMap<Command.ReactPhotoPost, CreateReactUserPhotoPostCommandResult>();
            CreateMap<Command.ReactVideoPost, CreateReactUserVideoPostCommandResult>();
            CreateMap<Command.ReactComment, CreateReactCommentUserPostCommandResult>();
            CreateMap<Command.ReactVideoPostComment, CreateReactCommentUserPostVideoCommandResult>();
            CreateMap<Command.ReactPhotoPostComment, CreateReactCommentUserPostPhotoCommandResult>();

            CreateMap<Query.ReactPost, CreateReactUserPostCommandResult>();
            CreateMap<Query.ReactType, CreateNewReactCommandResult>();
            CreateMap<Query.ReactPhotoPost, CreateReactUserPhotoPostCommandResult>();
            CreateMap<Query.ReactVideoPost, CreateReactUserVideoPostCommandResult>();
            CreateMap<Query.ReactComment, CreateReactCommentUserPostCommandResult>();
            CreateMap<Query.ReactVideoPostComment, CreateReactCommentUserPostVideoCommandResult>();
            CreateMap<Query.ReactPhotoPostComment, CreateReactCommentUserPostPhotoCommandResult>();
            CreateMap<Query.ReactType, GetAllReactTypeQueryResult>();

            //Notification
            CreateMap<Query.Notification, GetUserNotificationsListQueryResult>();
            CreateMap<Query.Notification, UpdateNotificationStatusResult>();
            CreateMap<NotificationOutDTO, GetUserNotificationsListQueryResult>();

            //GroupPost
            CreateMap<GroupPhotoDTO, GroupPhoto>().ReverseMap();
            CreateMap<GroupPostPhotoDTO, GroupPostPhoto>().ReverseMap();
            CreateMap<GroupVideoDTO, GroupVideo>().ReverseMap();
            CreateMap<GroupPostVideoDTO, GroupPostVideo>().ReverseMap();
            CreateMap<GroupFPTDTO, GroupFpt>();
            CreateMap<GetGroupStatusDTO, GroupStatus>();
            CreateMap<GroupPostDTO, GroupPost>().ReverseMap();
            CreateMap<Command.GroupSharePost, ShareGroupPostCommandResult>().ReverseMap();
            CreateMap<Query.GroupSharePost, ShareGroupPostCommandResult>().ReverseMap();
            CreateMap<Command.CommentGroupPost, UpdateCommentGroupPostCommandResult>().ReverseMap();
            CreateMap<Command.CommentGroupVideoPost, UpdateCommentGroupVideoPostCommandResult>().ReverseMap();
            CreateMap<Command.CommentPhotoGroupPost, UpdateCommentGroupPhotoPostCommandResult>().ReverseMap();
            CreateMap<Query.CommentGroupPost, UpdateCommentGroupPostCommandResult>().ReverseMap();
            CreateMap<Query.CommentGroupVideoPost, UpdateCommentGroupVideoPostCommandResult>().ReverseMap();
            CreateMap<Query.CommentPhotoGroupPost, UpdateCommentGroupPhotoPostCommandResult>().ReverseMap();

            //SharePost
            CreateMap<SharePost, ShareUserPostCommandResult>().ReverseMap();
            CreateMap<GroupSharePost, ShareGroupPostCommandResult>().ReverseMap();
            CreateMap<Command.ReactSharePost, CreateReactForSharePostCommandResult>().ReverseMap();
            CreateMap<Command.ReactGroupSharePost, CreateReactForGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Command.ReactSharePostComment, CreateReactForCommentSharePostCommandResult>().ReverseMap();
            CreateMap<Command.ReactGroupSharePostComment, CreateReactForCommentGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Query.ReactSharePost, CreateReactForSharePostCommandResult>().ReverseMap();
            CreateMap<Query.ReactGroupSharePost, CreateReactForGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Query.ReactSharePostComment, CreateReactForCommentSharePostCommandResult>().ReverseMap();
            CreateMap<Query.ReactGroupSharePostComment, CreateReactForCommentGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Command.CommentSharePost, UpdateCommentSharePostCommandResult>().ReverseMap();
            CreateMap<Command.CommentGroupSharePost, UpdateCommentGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Command.CommentGroupSharePost, CreateCommentForGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Command.GroupSharePost, UpdateGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Query.CommentSharePost, UpdateCommentSharePostCommandResult>().ReverseMap();
            CreateMap<Query.CommentGroupSharePost, UpdateCommentGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Query.CommentGroupSharePost, CreateCommentForGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Query.GroupSharePost, UpdateGroupSharePostCommandResult>().ReverseMap();
            CreateMap<Query.CommentGroupSharePost, GetCommentByGroupSharePostQueryResult>().ReverseMap();
            CreateMap<CommentSharePost, GetCommentBySharePostQueryResult>().ReverseMap();
            CreateMap<ReactSharePost, GetReactBySharePostQueryResult>().ReverseMap();
            CreateMap<ReactGroupSharePost, GetReactByGroupSharePostQueryResult>().ReverseMap();
            CreateMap<Command.CommentSharePost, CreateCommentForSharePostCommandResult>().ReverseMap();
            CreateMap<Command.SharePost, UpdateSharePostCommandResult>().ReverseMap();
            CreateMap<Query.SharePost, UpdateSharePostCommandResult>().ReverseMap();
        }
    }
}
