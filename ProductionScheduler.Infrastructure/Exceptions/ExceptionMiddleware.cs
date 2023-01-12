using Microsoft.AspNetCore.Http;
using ProductionScheduler.Core.Exceptions;

namespace ProductionScheduler.Infrastructure.Exceptions
{

    internal sealed class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }


        private async Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            var name = exception.GetType().Name.Replace("Exception", string.Empty);

            // przypisujesz do 2 wartosci 
            var (statusCode, error) = exception switch
            {   
                CustomException => (StatusCodes.Status400BadRequest, new Error(name, exception.Message)), 

                _ => (StatusCodes.Status500InternalServerError, new Error(name, "There was an error"))
            };

            context.Response.StatusCode = statusCode;
          //  context.
        }

        private record Error(string Code, string Reason);

    } 
}