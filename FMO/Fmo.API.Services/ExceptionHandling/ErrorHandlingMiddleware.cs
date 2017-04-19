using System;
using System.Net;
using System.Threading.Tasks;
using Fmo.Common.Enums;
using Fmo.Common.ExceptionManagement;
using Fmo.Common.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Fmo.API.Services.MiddlerWare
{
    /// <summary>
    /// Error Handling Middleware. Interceptor for handling error in controllers.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private IExceptionHelper exceptionHelper = default(IExceptionHelper);

        public ErrorHandlingMiddleware(RequestDelegate next, IExceptionHelper exceptionHelper)
        {
            this.next = next;
            this.exceptionHelper = exceptionHelper;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Exception newException;
                bool rethrow = exceptionHelper.HandleException(ex, ExceptionHandlingPolicy.LogAndWrap, out newException);
                if (rethrow)
                {
                    if (newException == null)
                    {
                        //throw;
                        await HandleExceptionAsync(context, ex);
                    }
                    else
                    {
                        //throw newException;
                        await HandleExceptionAsync(context, newException);
                    }
                }
                else
                {
                    //throw;
                    await HandleExceptionAsync(context, ex);
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code = default(HttpStatusCode);

            if (exception is EntityNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;
            else if (exception is NotImplementedException) code = HttpStatusCode.NotImplemented;
            else code = HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}