using Application.Services.CacheService;
using API.Configurations;
using StackExchange.Redis;

namespace API.Middlewares.Extentions
{
    public class CacheExtentions : IExtentions
    {
        public void ExtentionServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = new RedisConfiguration();
            configuration.GetSection("RedisConfifuration").Bind(redisConfig);

            services.AddSingleton(redisConfig);

            if (!redisConfig.Enable)
                return;

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfig.ConnectionString));
            services.AddStackExchangeRedisCache(option => option.Configuration = redisConfig.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
