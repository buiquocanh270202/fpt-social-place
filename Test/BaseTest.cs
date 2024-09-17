using AutoFixture;
using Domain.CommandModels;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public abstract class BaseTest : IDisposable
    {
        protected readonly string userName = "UserName";
        protected readonly string bearerToken = "Bearer Token";
        private readonly Fixture fixture;
        protected readonly fptforumCommandContext _commandContext;
        protected readonly fptforumQueryContext _queryContext;

        public BaseTest()
        {
            fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var commandOptions = new DbContextOptionsBuilder<fptforumCommandContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var queryOptions = new DbContextOptionsBuilder<fptforumQueryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _commandContext = new fptforumCommandContext(commandOptions);
            _queryContext = new fptforumQueryContext(queryOptions);
            _commandContext.Database.EnsureCreated();
            _queryContext.Database.EnsureCreated();

        }
        public void Dispose()
        {
            _commandContext.Dispose();
            _queryContext.Dispose();
        }
    }
}
