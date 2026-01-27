using Microsoft.AspNetCore.Diagnostics;

namespace OrderService.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _logger.LogError("{ExceptionType}:{ExceptionMessage}",ex.InnerException.GetType().ToString(),ex.InnerException.Message);
                }
                else
                {
                     _logger.LogError("{ExceptionType}:{ExceptionMessage}",ex.GetType().ToString(),ex.Message);
                }
            }
            
        }

    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlerMiddleware>();
        return builder;
    }
}