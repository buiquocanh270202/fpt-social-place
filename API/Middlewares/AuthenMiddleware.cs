using Domain.CommandModels;
using Domain.QueryModels;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Net.Http.Headers;

namespace API.Middlewares
{
    public class AuthenMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;
        private readonly ILogger<AuthenMiddleware> _logger;

        public AuthenMiddleware(RequestDelegate next, IAntiforgery antiforgery, ILogger<AuthenMiddleware> logger)
        {
            _next = next;
            _antiforgery = antiforgery;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post)
            {
                var authHeader = context.Request.Headers[HeaderNames.Authorization].ToString();

                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Bearer token found in Authorization header.");

                    // Validate the XSRF token
                    try
                    {
                        await _antiforgery.ValidateRequestAsync(context);
                    }
                    catch (AntiforgeryValidationException ex)
                    {
                        _logger.LogError(ex, "Invalid XSRF token.");
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Invalid XSRF token.");
                        return;
                    }
                }
            }

            await _next(context);
        }

    }
}
