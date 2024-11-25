using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CacheService
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsyns(string cacheKey, object response, TimeSpan expireTime);
        Task<string> GetCacheResponseAsyns(string cacheKey);
        Task RemoveCacheResponseAsyns(string partern);
        public string GenerateCacheKeyFromRequest(HttpRequest request);
    }
}
