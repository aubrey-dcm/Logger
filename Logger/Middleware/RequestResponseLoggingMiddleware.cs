using Microsoft.AspNetCore.Http;

namespace Logger.Middleware
{
    public class RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
        }
    }
}