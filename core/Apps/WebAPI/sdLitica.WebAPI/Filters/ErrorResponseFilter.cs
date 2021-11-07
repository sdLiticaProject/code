using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using sdLitica.Exceptions.Abstractions;

namespace sdLitica.Filters
{
    /// <summary>
    /// filter that process exceptions
    /// </summary>
    public class ErrorResponseFilter : IExceptionFilter
    {
        /// <summary>
        /// called when exception thrown
        /// </summary>
        /// <param name="context">exception context</param>
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            HttpResponse response = context.HttpContext.Response;

            response.ContentType = "application/json";

            if (context.Exception is BaseExceptionModel exceptionModel)
            {
                // todo to proper logger
                Console.WriteLine($"[{DateTimeOffset.UtcNow}] {exceptionModel.ToJson()}");
                response.StatusCode = (int)exceptionModel.StatusCode;
                response.WriteAsync(exceptionModel.ToJson());
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Console.WriteLine($"[{DateTimeOffset.UtcNow}] {context.Exception}");
                response.WriteAsync(context.Exception.Message);
            }
        }
    }

}
