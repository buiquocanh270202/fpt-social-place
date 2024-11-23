using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;
using Application.Commands.CreateNotifications;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Core.Helper;
using Microsoft.EntityFrameworkCore.Query;
using Application.Queries.CheckUserExist;
using AutoFixture;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq.Expressions;

namespace Test.SWT_Lab3
{
    [TestFixture]
    public class CreateNotificationsTests : GuidHelper
    {
        private Mock<fptforumCommandContext> _commandContextMock;
        private Mock<fptforumQueryContext> _queryContextMock;
        private Mock<GuidHelper> _guidHelperMock;
        private CreateNotifications _createNotifications;
        private Mock<IServiceProvider> _serviceProviderMock;
        

        [SetUp]
        public void SetUp()
        {
            _commandContextMock = new Mock<fptforumCommandContext>();
            _queryContextMock = new Mock<fptforumQueryContext>();
            _guidHelperMock = new Mock<GuidHelper>();
            _serviceProviderMock = new Mock<IServiceProvider>();

            _createNotifications = new CreateNotifications(_serviceProviderMock.Object)
            {
                _commandContext = _commandContextMock.Object,
                _queryContext = _queryContextMock.Object

            };
        }

        [Test]
        public async Task UTCID01_CreateNotification_ShouldThrowException_WhenQueryContextIsNull()
        {
            // Arrange
            _createNotifications._queryContext = null;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ErrorException>(async () =>
                await _createNotifications.CreateNotitfication("19313e4e-f238-486a-a62d-e3c0dc7d1c5d", "14484550-d72c-401e-b403-f950cde67639", "message", "url")
            );

            Assert.That(ex.StatusCode, Is.EqualTo(StatusCodeEnum.Context_Not_Found));
        }

        [Test]
        public async Task UTCID02_CreateNotification_ShouldThrowException_WhenUserNotFound()
        {

            // Arrange
            var emptyUserProfiles = new List<Domain.QueryModels.UserProfile>().AsQueryable();

            // Create a mock of IAsyncQueryProvider
            var asyncQueryProviderMock = new Mock<IAsyncQueryProvider>();
            asyncQueryProviderMock.Setup(p => p.Execute(
                It.IsAny<Expression>()))
                .Returns((Domain.QueryModels.UserProfile)null);

            // Set up the query provider for the mock DbSet
            var userProfileDbSetMock = new Mock<DbSet<Domain.QueryModels.UserProfile>>();
            userProfileDbSetMock.As<IQueryable<Domain.QueryModels.UserProfile>>().Setup(m => m.Provider).Returns(asyncQueryProviderMock.Object);
            userProfileDbSetMock.As<IQueryable<Domain.QueryModels.UserProfile>>().Setup(m => m.Expression).Returns(emptyUserProfiles.Expression);
            userProfileDbSetMock.As<IQueryable<Domain.QueryModels.UserProfile>>().Setup(m => m.ElementType).Returns(emptyUserProfiles.ElementType);
            userProfileDbSetMock.As<IQueryable<Domain.QueryModels.UserProfile>>().Setup(m => m.GetEnumerator()).Returns(emptyUserProfiles.GetEnumerator());

            // Set up the query context to return the mocked DbSet
            _queryContextMock.Setup(q => q.UserProfiles).Returns(userProfileDbSetMock.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ErrorException>(async () =>
                await _createNotifications.CreateNotitfication("19313e4e-f238-486a-a62d-e3c0dc7d1c5d", "059a1de4-b630-4240-a7a3-a16e4f7bbf75", "message", "url")
            );

            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.StatusCode, Is.EqualTo(StatusCodeEnum.Error));


        }

        

        [Test]
        public async Task UTCID03_CreateNotification_ShouldThrowException_WhenNotificationTypeNotFound()
        {
            // Arrange
            var userId = "14484550-d72c-401e-b403-f950cde67639";
            var senderId = "19313e4e-f238-486a-a62d-e3c0dc7d1c5d";
            var userStatusId = Guid.NewGuid();

            var userProfile = new Domain.QueryModels.UserProfile
            {
                UserId = Guid.Parse(userId),
                UserStatusId = userStatusId,
                FirstName = "John",
                LastName = "Doe"
            };

            var notificationTypes = new List<Domain.QueryModels.NotificationType>().AsQueryable();

            var userProfiles = new List<Domain.QueryModels.UserProfile> { userProfile }.AsQueryable();

            // Mock the UserProfiles DbSet
            var userProfilesMock = DbSetMockHelper.CreateAsyncDbSetMock(userProfiles);
            // Mock the NotificationTypes DbSet
            var notificationTypesMock = DbSetMockHelper.CreateAsyncDbSetMock(notificationTypes);

            // Set up the query context to return the mocked DbSets
            _queryContextMock.Setup(q => q.UserProfiles).Returns(userProfilesMock.Object);
            _queryContextMock.Setup(q => q.NotificationTypes).Returns(notificationTypesMock.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ErrorException>(async () =>
                await _createNotifications.CreateNotitfication(senderId, userId, "message", "url", NotificationsTypeEnum.IMPORTANCE)
            );

            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.StatusCode, Is.EqualTo(StatusCodeEnum.NT01_Not_Found));
        }


        [Test]
        public async Task UTCID04_CreateNotification_ShouldCreateSuscess_WhenNotAsignEnum()
        {

            // Arrange
            var userId = "14484550-d72c-401e-b403-f950cde67639";
            var senderId = "19313e4e-f238-486a-a62d-e3c0dc7d1c5d";
            var notificationTypeId = Guid.NewGuid();
            var newNotificationId = Guid.NewGuid();


            var notificationType = new Domain.QueryModels.NotificationType
            {
                NotificationTypeId = notificationTypeId,
                NotificationTypeName = "Normal"
            };

            var userProfile = new Domain.QueryModels.UserProfile
            {
                UserId = Guid.Parse(userId),
                FirstName = "John",
                LastName = "Doe"
            };

            var notificationTypes = new List<Domain.QueryModels.NotificationType> { notificationType }.AsQueryable();

            var userProfiles = new List<Domain.QueryModels.UserProfile> { userProfile }.AsQueryable();

            // Mock the UserProfiles DbSet
            var userProfilesMock = DbSetMockHelper.CreateAsyncDbSetMock(userProfiles);
            // Mock the NotificationTypes DbSet
            var notificationTypesMock = DbSetMockHelper.CreateAsyncDbSetMock(notificationTypes);

            // Set up the query context to return the mocked DbSets
            _queryContextMock.Setup(q => q.UserProfiles).Returns(userProfilesMock.Object);
            _queryContextMock.Setup(q => q.NotificationTypes).Returns(notificationTypesMock.Object);

            var notificationsDbSetMock = new Mock<DbSet<Domain.CommandModels.Notification>>();
            _commandContextMock.Setup(c => c.Notifications).Returns(notificationsDbSetMock.Object);

            // Act
            await _createNotifications.CreateNotitfication(senderId, userId, "message", "url", NotificationsTypeEnum.NORMAL);

            // Assert
            notificationsDbSetMock.Verify(n => n.Add(It.Is<Domain.CommandModels.Notification>(notif =>
                notif.UserId == Guid.Parse(userId) &&
                notif.SenderId == Guid.Parse(senderId) &&
                notif.NotiMessage == "message" &&
                notif.NotifiUrl == "url" &&
                notif.IsRead == false
            )), Times.Once);
        }


        [Test]
        public async Task UTCID05_CreateNotification_ShouldThrowExNotFoundNotiType_WhenNotAsignEnum()
        {
            // Arrange
            var userId = "14484550-d72c-401e-b403-f950cde67639";
            var senderId = "19313e4e-f238-486a-a62d-e3c0dc7d1c5d";
            var notificationTypeId = Guid.NewGuid();


            var notificationType = new Domain.QueryModels.NotificationType
            {
                NotificationTypeId = notificationTypeId,
                NotificationTypeName = "Importan"
            };

            var userProfile = new Domain.QueryModels.UserProfile
            {
                UserId = Guid.Parse(userId),
                FirstName = "John",
                LastName = "Doe"
            };



            var notificationTypes = new List<Domain.QueryModels.NotificationType> { notificationType }.AsQueryable();

            var userProfiles = new List<Domain.QueryModels.UserProfile> { userProfile }.AsQueryable();

            // Mock the UserProfiles DbSet
            var userProfilesMock = DbSetMockHelper.CreateAsyncDbSetMock(userProfiles);
            // Mock the NotificationTypes DbSet
            var notificationTypesMock = DbSetMockHelper.CreateAsyncDbSetMock(notificationTypes);

            // Set up the query context to return the mocked DbSets
            _queryContextMock.Setup(q => q.UserProfiles).Returns(userProfilesMock.Object);
            _queryContextMock.Setup(q => q.NotificationTypes).Returns(notificationTypesMock.Object);

            var notificationsDbSetMock = new Mock<DbSet<Domain.CommandModels.Notification>>();
            _commandContextMock.Setup(c => c.Notifications).Returns(notificationsDbSetMock.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ErrorException>(async () =>
            await _createNotifications.CreateNotitfication(senderId, userId , "message", "url")
);

            Assert.That(ex, Is.Not.Null);
            Assert.That(ex.StatusCode, Is.EqualTo(StatusCodeEnum.NT01_Not_Found));

        }


        [Test]
        public async Task UTCID06_CreateNotification_ShouldAddNotification_WhenUserAndNotificationTypeAreFound()
        {
            // Arrange
            var userId = "14484550-d72c-401e-b403-f950cde67639";
            var senderId = "19313e4e-f238-486a-a62d-e3c0dc7d1c5d";
            var notificationTypeId = Guid.NewGuid();
            var newNotificationId = Guid.NewGuid();
           

            var notificationType = new Domain.QueryModels.NotificationType
            {
                NotificationTypeId = notificationTypeId,
                NotificationTypeName = "Normal"
            };

            var userProfile = new Domain.QueryModels.UserProfile
            {
                UserId = Guid.Parse(userId),
                FirstName = "John",
                LastName = "Doe"
            };



            var notificationTypes = new List<Domain.QueryModels.NotificationType> { notificationType }.AsQueryable();

            var userProfiles = new List<Domain.QueryModels.UserProfile> { userProfile }.AsQueryable();

            // Mock the UserProfiles DbSet
            var userProfilesMock = DbSetMockHelper.CreateAsyncDbSetMock(userProfiles);
            // Mock the NotificationTypes DbSet
            var notificationTypesMock = DbSetMockHelper.CreateAsyncDbSetMock(notificationTypes);

            // Set up the query context to return the mocked DbSets
            _queryContextMock.Setup(q => q.UserProfiles).Returns(userProfilesMock.Object);
            _queryContextMock.Setup(q => q.NotificationTypes).Returns(notificationTypesMock.Object);

            var notificationsDbSetMock = new Mock<DbSet<Domain.CommandModels.Notification>>();
            _commandContextMock.Setup(c => c.Notifications).Returns(notificationsDbSetMock.Object);

            // Act
            await _createNotifications.CreateNotitfication("19313e4e-f238-486a-a62d-e3c0dc7d1c5d", "14484550-d72c-401e-b403-f950cde67639", "message", "url");

            // Assert
            notificationsDbSetMock.Verify(n => n.Add(It.Is<Domain.CommandModels.Notification>(notif =>
                notif.UserId == Guid.Parse(userId) &&
                notif.SenderId == Guid.Parse(senderId) &&
                notif.NotiMessage == "message" &&
                notif.NotifiUrl == "url" &&
                notif.IsRead == false
            )), Times.Once);

            //_commandContextMock.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}

