using Microsoft.AspNetCore.Diagnostics;
using OrderService.Api.Middlewares;

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

                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsJsonAsync(new { Type = ex.GetType().ToString(), Message = ex.Message });
            }
            
        }

    }
}

public static class ExceptionHandlingMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
        return builder;
    }
}