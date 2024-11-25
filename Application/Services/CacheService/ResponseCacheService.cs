using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CacheService
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiple;

        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiple)
        {
            _distributedCache = distributedCache;
            _connectionMultiple = connectionMultiple;
        }
        
        public async Task<string> GetCacheResponseAsyns(string cacheKey)
        {
            var cacheResponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
        }

        public async Task  RemoveCacheResponseAsyns(string pattern)
        {
            if(string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentNullException("Value can not null");

            await foreach (var key in GetKeyAsync(pattern + "*"))
            {
                await _distributedCache.RemoveAsync(key);
            }
        }

        private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentNullException("Value can not null");

            foreach (var endPoint in _connectionMultiple.GetEndPoints()) 
            { 
                var server = _connectionMultiple.GetServer(endPoint);
                foreach(var key in server.Keys(pattern: pattern))
                {
                    yield return key.ToString();
                }
            }

        }

        public async Task SetCacheResponseAsyns(string cacheKey, object response, TimeSpan expireTime)
        {
            if (response == null)
                return;

            var serializeResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(cacheKey, serializeResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expireTime,
            });
        }

        public string  GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");

            }
            return keyBuilder.ToString();
        }
    }
}
