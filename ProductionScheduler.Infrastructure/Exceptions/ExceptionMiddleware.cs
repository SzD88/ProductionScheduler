using Humanizer;
using Microsoft.AspNetCore.Http;
using ProductionScheduler.Application.Services;
using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Infrastructure.Exceptions
{
    // cuz it is internal ! #refactor
    internal sealed class ExceptionMiddleware : IMiddleware
    {

        //public ExceptionMiddleware(IServiceProvider prov)
        //{

        //}
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
           // catch (CustomException ce) // tej moj #refactor usun komentarz
            //{ 
            
            //}
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
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
                _ => (StatusCodes.Status500InternalServerError, new Error(name, "There was an error"))
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(error);
        }

        private record Error(string Code, string Reason);

    } 
}