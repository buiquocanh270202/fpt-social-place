using Application.DTO.FriendDTO;
using Application.Queries.FindUserByName;
using AutoFixture;
using AutoMapper;
using Domain.QueryModels;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Test.Queries
{
    public class FindUserByNameQueryHandlerTest : BaseTest
    {
        private readonly FindUserByNameQueryHandler _queryHandler;
        private readonly Fixture _fixture;
        private readonly Mock<fptforumQueryContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;

        public FindUserByNameQueryHandlerTest()
        {
            _fixture = new Fixture();
            _mockContext = new Mock<fptforumQueryContext>();
            _mockMapper = new Mock<IMapper>();
            _queryHandler = new FindUserByNameQueryHandler(_mockContext.Object, _mockMapper.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var users = new List<UserProfile>
            {
                new UserProfile
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "John Doe",
                    IsActive = true,
                    AvataPhotos = new List<AvataPhoto>
                    {
                        new AvataPhoto { IsUsed = true, AvataPhotosUrl = "john_photo.jpg" }
                    }
                },
                new UserProfile
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "Jane Doe",
                    IsActive = true,
                    AvataPhotos = new List<AvataPhoto>
                    {
                        new AvataPhoto { IsUsed = true, AvataPhotosUrl = "jane_photo.jpg" }
                    }
                }
            };

            var userProfilesDbSet = CreateMockDbSet(users);
            _mockContext.Setup(m => m.UserProfiles).Returns(userProfilesDbSet.Object);
        }

        [Fact]
        public async Task FindUserByName_ShouldReturnExactMatch_WhenUserExists()
        {
            var id = Guid.NewGuid();
            // Arrange
            var query = _fixture.Build<FindUserByNameQuery>()
                .With(q => q.FindName, "John Doe")
                .With(q => q.UserId, id)
                .Create();

            // Act
            var result = await _queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value.ListFriend); // John Doe should be in the ListFriend
            Assert.Equal("John Doe", result.Value.ListFriend.First().FriendName);
        }

        [Fact]
        public async Task FindUserByName_ShouldReturnEmptyResult_WhenUserNotFound()
        {
            // Arrange
            var query = _fixture.Build<FindUserByNameQuery>()
                .With(q => q.FindName, "Nonexistent User")
                .Create();

            // Act
            var result = await _queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value.ListFriend); // No match found
            Assert.Empty(result.Value.ListUserNotFriend); // No match found
        }

        [Fact]
        public async Task FindUserByName_ShouldHandleDiacriticsAndCaseInsensitiveMatches()
        {
            // Arrange
            var query = _fixture.Build<FindUserByNameQuery>()
                .With(q => q.FindName, "fohn") // Simulating diacritics in the search
                .Create();

            // Act
            var result = await _queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value.ListFriend); // John Doe should be matched
            Assert.Equal("John Doe", result.Value.ListFriend.First().FriendName);
        }

        [Fact]
        public async Task FindUserByName_ShouldThrowContextNotFoundException_WhenContextIsNull()
        {
            // Arrange
            var queryHandler = new FindUserByNameQueryHandler(null, _mockMapper.Object);
            var query = _fixture.Create<FindUserByNameQuery>();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ErrorException>(() => queryHandler.Handle(query, CancellationToken.None));
            Assert.Equal(StatusCodeEnum.Context_Not_Found, exception.StatusCode);
        }

        [Fact]
        public async Task FindUserByName_ShouldReturnUserNotInFriendList_WhenTheyAreNotFriends()
        {
            // Arrange
            var query = _fixture.Build<FindUserByNameQuery>()
                .With(q => q.FindName, "Jane Doe")
                .Create();

            // Act
            var result = await _queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value.ListUserNotFriend); // Jane Doe should be in the ListUserNotFriend
            Assert.Equal("Jane Doe", result.Value.ListUserNotFriend.First().FriendName);
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> entities) where T : class
        {
            var queryable = entities.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return mockSet;
        }
    }
}
