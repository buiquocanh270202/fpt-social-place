using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandModel = Domain.CommandModels;
using Domain.Exceptions;
using AutoMapper;
using Application.Commands.CreateUserProfile;
using Application.Commands.GetUserProfile;
using Application.DTO.CreateUserDTO;
using Domain.Enums;
using Xunit;
using Moq;
using Core.Helper;
using Application.Mappers;

namespace Test.Command
{
    public class CreateUserProfileTest : BaseTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserProfileCommandHandler _handler;
        private readonly GuidHelper _helper;
        private UserProfileCommand command;
        private List<CreateUserSettingDTO> userSettings;
        private List<CreateUserInteresDTO> userInterests;
        private List<CreateUserWorkPlaceDTO> userWorkPlaces;
        private List<CreateUserWebAffilicationDTO> userWebAffilications;

        public CreateUserProfileTest()
        {
            _helper = new GuidHelper();

            _mockMapper = new Mock<IMapper>();
            _handler = new UserProfileCommandHandler(_commandContext, _queryContext, _mockMapper.Object);
            Setup();
        }


        public void Setup()
        {

            userSettings = new List<CreateUserSettingDTO>
            {
                new CreateUserSettingDTO
                {
                    SettingId = new Guid("ae10b6ce-2c70-4ca3-ab4f-b742a868c88f"),
                }
                
            };

            userInterests = new List<CreateUserInteresDTO>
            {
                new CreateUserInteresDTO
                {
                    InterestId = Guid.NewGuid(),
                    
                },
                new CreateUserInteresDTO
                {
                    InterestId = Guid.NewGuid(),
                }
            };

            userWorkPlaces = new List<CreateUserWorkPlaceDTO>
            {
                new CreateUserWorkPlaceDTO
                {
                    WorkPlaceName = "TechCorp"
                },
                new CreateUserWorkPlaceDTO
                {
                  WorkPlaceName = "Ha Noi"
                }
            };

            userWebAffilications = new List<CreateUserWebAffilicationDTO>
            {
                new CreateUserWebAffilicationDTO
                {
                    WebAffiliationUrl = "https://www.linkedin.com/in/username"
                },
                new CreateUserWebAffilicationDTO
                {
                    WebAffiliationUrl = "https://github.com/username"
                },
                new CreateUserWebAffilicationDTO
                {
                    WebAffiliationUrl = "https://www.personalblog.com"
                }
            };

            command = new UserProfileCommand
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "existing@example.com",
                FeId = "12345",
                RoleName = "User",
                BirthDay = new DateTime(1990, 1, 1),
                Gender = new CreateUserGenderDTO { GenderId = Guid.NewGuid() },
                ContactInfor = new CreateUserContactInforDTO
                {
                    SecondEmail = "john.secondary@example.com",
                    PrimaryNumber = "1234567890",
                    SecondNumber = "0987654321"
                },
                Relationship = new CreateUserRelationshipDTO { RelationshipId = Guid.NewGuid() },
                AboutMe = "About John",
                HomeTown = "Hometown",
                CoverImage = "cover.jpg",
                UserNumber = "1001",
                Avataphoto = "avatar.jpg",
                Campus = "Campus",
                UserSetting = userSettings,
                Interes = userInterests,
                WorkPlace = userWorkPlaces,
                WebAffilication = userWebAffilications
            };
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldCreateUserProfile()
        {
            var userProfile = new CommandModel.UserProfile
            {
                UserId = command.UserId.Value,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                BirthDay = command.BirthDay,
                RoleId = Guid.NewGuid(),
                UserStatusId = Guid.NewGuid() 
            };

            _mockMapper.Setup(m => m.Map<CommandModel.UserProfile>(It.IsAny<UserProfileCommand>()))
           .Returns(userProfile);
            _mockMapper.Setup(m => m.Map<UserProfileCommandResult>(It.IsAny<UserProfileCommand>()))
           .Returns(new UserProfileCommandResult());

            _queryContext.Roles.Add(new Domain.QueryModels.Role { RoleId = userProfile.RoleId, NameRole = command.RoleName });

            _queryContext.UserStatuses.Add(new Domain.QueryModels.UserStatus { UserStatusId = userProfile.UserStatusId, StatusName = "Public" });
            _queryContext.Settings.Add(
                new Domain.QueryModels.Setting
                {
                    SettingId = new Guid("ae10b6ce-2c70-4ca3-ab4f-b742a868c88f"),
                    SettingName = "Profile Status"
                });
            _queryContext.SaveChanges();
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.UserId, result.Value.UserId);
        }

        [Fact]
        public async Task Handle_GivenExistingEmail_ShouldThrowException()
        {
            // Arrange
            var existingUser = new Domain.QueryModels.UserProfile
            {
                UserId = Guid.NewGuid(),
                FirstName = "Existing",
                LastName = "User",
                Email = "existing@example.com",
                FeId = "12345",
                RoleId = Guid.NewGuid(),
                UserStatusId = Guid.NewGuid()
            };
            await _queryContext.UserProfiles.AddAsync(existingUser);
            await _queryContext.SaveChangesAsync();

           
            await Assert.ThrowsAsync<ErrorException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_UserProfileCommand_ThrowsErrorExceptionWhenContextIsNull()
        {
            var command = new UserProfileCommand();
            var handler = new UserProfileCommandHandler(null, _queryContext, _mockMapper.Object);
            await Assert.ThrowsAsync<ErrorException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
