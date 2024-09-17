using Application.Queries.CheckUserExist;
using AutoFixture;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using System.Reflection.Metadata;
using Query = Domain.QueryModels;
using Domain.Exceptions;

namespace Test.Queries
{
    public class CheckExistQueryHandlerTest : BaseTest
    {
        private readonly CheckUserExistHandler _query;
        private Fixture _fixture;
        public CheckExistQueryHandlerTest()
        {
            _fixture = new Fixture();
            _query = new CheckUserExistHandler(_queryContext);
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _queryContext.UserProfiles.Add(new Query.UserProfile
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                IsActive = true
            });

            _queryContext.UserProfiles.Add(new Query.UserProfile
            {
                UserId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                IsActive = false
            });

            _queryContext.SaveChanges();
        }

        [Fact]
        public async Task CheckExistTestCase1()
        {
            var query = _fixture.Create<CheckUserExistQuery>();
            var result = await _query.Handle(query, cancellationToken: default);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CheckExistTestCase2()
        {
            var existingUser = _queryContext.UserProfiles.FirstOrDefault(x => x.IsActive);
            var query = new CheckUserExistQuery { UserId = existingUser.UserId };
            var result = await _query.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(StatusCodeEnum.U03_User_Exist, result.Value.enumcode);
            Assert.Equal(existingUser.UserId, result.Value.UserId);
            Assert.Equal(existingUser.Email, result.Value.Email);
        }

        [Fact]
        public async Task CheckExistTestCase3()
        {
            var existingUser = _queryContext.UserProfiles.FirstOrDefault(x => x.IsActive == false);
            var query = new CheckUserExistQuery { UserId = existingUser.UserId };
            var exception = await Assert.ThrowsAsync<ErrorException>(() => _query.Handle(query, CancellationToken.None));
            Assert.Equal(StatusCodeEnum.U06_User_Not_Active, exception.StatusCode);
        }
    }
}
