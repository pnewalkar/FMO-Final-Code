using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Fmo.Common.ExceptionManagement;
using Newtonsoft.Json;
using Fmo.Common.Interface;
using Fmo.Common.Enums;

namespace Fmo.API.Services.MiddlerWare
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private  IExceptionHelper exceptionHelper = default(IExceptionHelper);

        public ErrorHandlingMiddleware(RequestDelegate next, IExceptionHelper exceptionHelper)
        {
            this.next = next;
            this.exceptionHelper = exceptionHelper;
        }

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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code = default(HttpStatusCode);

            if (exception is EntityNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;
            else code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
