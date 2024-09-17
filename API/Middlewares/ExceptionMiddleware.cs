using Domain.ApiModels;
using Domain.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ErrorException ex)
            {

                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponseModel(ex.StatusCode, ex.Data), serializerSettings));
            }
            catch (UnauthorizedException ex)
            {

                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponseModel(ex.StatusCode, ex.Data), serializerSettings));
            }
            catch (ForbiddenException ex)
            {

                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponseModel(ex.StatusCode, ex.Data), serializerSettings));
            }
            catch (UnauthorizedAccessException ex)
            {

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExcetionAsync(httpContext, ex);
            }
        }

        private async Task HandleExcetionAsync(HttpContext context, Exception exception)
        {
            var request = context.Request;
            var requestMethod = request.Method;
            var date = DateTime.Now.ToLongDateString();
            var time = DateTime.Now.ToLongTimeString();
            var path = request.Path;
            var logMessage = $"{date} {time} |{LogLevel.Error} | method={requestMethod} | path={path} | message={exception.Message} | {exception.StackTrace.Trim()}";
            _logger.LogError(logMessage);
            Console.WriteLine(exception.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(exception.ToString());
        }
    }
}
