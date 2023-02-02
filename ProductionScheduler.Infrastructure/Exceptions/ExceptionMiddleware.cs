using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Infrastructure.Exceptions
{
    // cuz it is internal ! #refactor
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
                // Console.WriteLine(exception.ToString()); replaced with logger
                _logger.LogError(exception, exception.Message);
                await HandleExceptionAsync(exception, context);
            }
        } 
        private async Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            //extension Humanizer - underscore 
            var name = exception.GetType().Name.Underscore().Replace("_exception", string.Empty);

            // przypisujesz do 2 wartosci 
            var (statusCode, error) = exception switch
            {   // jezeli jest to ustawiony przez ciebie blad - wtedy zwraca bad request i tresc przez ciebie zaplanowana
                CustomException => (StatusCodes.Status400BadRequest, new Error(name, exception.Message)), 
                // jezeli to jest kazdy inny blad wtedy zwraca tylko informacje ze to error bo to moze byc niebezpieczy wyciek danych #refactor 
                _ => (StatusCodes.Status500InternalServerError, new Error(name, $"There was an error + {exception.Message}"))
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(error);
        }

        private record Error(string Code, string Reason);

    } 
}