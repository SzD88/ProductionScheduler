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
                await next(context); // probujesz w middleware dzialac dalej - czyli wykonywac kolejne kroki middle ware wg ich kolejnosci
                // jezeli ok to ok ale jezeli nie ok i cos wyrzuca exception to wylapany exception jest wrzucany w handle  i wg moich zasad rozwiazywany
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

            // przypisujesz do 2 wartosci  #refactor good to remember
            var (statusCode, error) = exception switch
            {   // jezeli jest to ustawiony przez ciebie blad - wtedy zwraca bad request i tresc przez ciebie zaplanowana
                CustomException => (StatusCodes.Status400BadRequest, new Error(name, exception.Message)),  //custom exception to moj wlasny typ 
                // jezeli to jest kazdy inny blad wtedy zwraca tylko informacje ze to error bo to moze byc niebezpieczy wyciek danych #refactor 
                _ => (StatusCodes.Status500InternalServerError, new Error(name, $"There was an error + {exception.Message}")) // refactor usun message - bo to nie powinno byc widoczne
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(error);
        }

        private record Error(string Code, string Reason);

    } 
}