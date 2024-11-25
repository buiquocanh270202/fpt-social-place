using API.Configurations;
using Application.Services.CacheService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API.Middlewares.Attributes
{
    public class CacheAttributes : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;
        //private readonly ResponseCacheService _responseCacheService;

        public CacheAttributes(ResponseCacheService responseCacheService, int expireTime = 1000)
        {
            _expireTime = expireTime;
            //_responseCacheService = responseCacheService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheConfig = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();

            if (!cacheConfig.Enable)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = cacheService.GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse =  await cacheService.GetCacheResponseAsyns(cacheKey);

            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };  
                context.Result = contentResult;
                return;
            }
            var excutedContent = await next();

            if (excutedContent.Result is OkObjectResult objectResult) { 
            await cacheService.SetCacheResponseAsyns(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_expireTime));

            }
        }
    }
}
