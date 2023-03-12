using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Infrastructure.Exceptions
{ 
    internal sealed class ExceptionMiddleware : IMiddleware
    {
        public ILogger<ExceptionMiddleware> _logger { get; }

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger )
        {
            _logger = logger;
        } 
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);  
            } 
            catch (Exception exception)
            { 
                _logger.LogError(exception, exception.Message);
                await HandleExceptionAsync(exception, context);
            }
        } 
        private async Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            
            var name = exception.GetType().Name.Underscore().Replace("_exception", string.Empty);
             
            var (statusCode, error) = exception switch
            {  
                CustomException => (StatusCodes.Status400BadRequest, new Error(name, exception.Message)),
                _ => (StatusCodes.Status500InternalServerError, new Error(name, $"There was an internal service error")) 
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(error);
        } 
        private record Error(string Code, string Reason);

    } 
}