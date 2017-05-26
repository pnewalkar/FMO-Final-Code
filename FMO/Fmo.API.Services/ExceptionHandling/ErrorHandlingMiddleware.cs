using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fmo.API.Services.ExceptionHandling.ResponseExceptionHandler;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.Common.ExceptionManagement;
using Fmo.Common.Interface;
using Fmo.Common.ResourceFile;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Fmo.API.Services.ExceptionHandling
{
    /// <summary>
    /// Error Handling Middleware. Interceptor for handling all the errors in controllers.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private IExceptionHelper exceptionHelper;
        private ILoggingHelper loggingHelper;
        private readonly RequestDelegate _next;
        private readonly ResponseExceptionHandlerOptions _options;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;

        public ErrorHandlingMiddleware(
            RequestDelegate next, 
            IExceptionHelper exceptionHelper,
            ILoggingHelper loggingHelper,
            IOptions<ResponseExceptionHandlerOptions> options)
        {
            this.next = next;
            this.exceptionHelper = exceptionHelper;
            this.loggingHelper = loggingHelper;
            this._options = options.Value;
            this._clearCacheHeadersDelegate = ClearCacheHeaders;
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
                exceptionHelper.HandleException(ex, ExceptionHandlingPolicy.LogAndWrap);
                if (ex.InnerException == null)
                {
                    await WriteExceptionToContextAsync(context, ex);
                }
                else
                {
                    await WriteExceptionToContextAsync(context, ex.InnerException);
                }
                //if (rethrow)
                //{
                //    if (wrappedException == null)
                //    {
                //        await HandleExceptionAsync(context, ex);
                //    }
                //    else
                //    {
                //        await HandleExceptionAsync(context, wrappedException);
                //    }
                //}
                //else
                //{
                //    await HandleExceptionAsync(context, ex);
                //}
            }
        }


        /// <summary>
        /// Handles the exception. Writes exception details to response object.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        private Task WriteExceptionToContextAsync(HttpContext context, Exception exception)
        {
            try
            {
                ExceptionResponse exceptionResponse;
                object response;
                if (_options.Responses.TryGetValue(exception.GetType(), out exceptionResponse))
                {
                    context.Response.StatusCode = exceptionResponse.StatusCode; // Replace default status code with actual.
                    response = GetExceptionResponse(exception, exceptionResponse);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var errorCode = GenerateErrorCode(_options.ErrorCodePrefix);
                    response = CreateErrorResponse(errorCode, _options.DefaultErrorMessage);
                }

                var result = JsonConvert.SerializeObject(response, _options.SerializerSettings);
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(result);
            }
            catch (Exception)
            {
                loggingHelper.LogError(ErrorMessageIds.Err_ExecutingErrorHandler, exception);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = exception.Message }));
            }
        }

        /// <summary>
        /// Gets the exception response.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="exceptionResponse">The exception response.</param>
        /// <returns></returns>
        private object GetExceptionResponse(Exception ex, ExceptionResponse exceptionResponse)
        {
            if (!string.IsNullOrWhiteSpace(exceptionResponse.Message))
            {
                return CreateErrorResponse(exceptionResponse.Message);
            }

            if (ex.Data.Count > 0)
            {
                string userFriendlyMessage = ex.Data["userFriendlyMessage"].ToString();
                return exceptionResponse.Response ?? CreateErrorResponse(userFriendlyMessage.Replace(Environment.NewLine, " "));
            }

            return exceptionResponse.Response ?? CreateErrorResponse(ex.Message.Replace(Environment.NewLine, " "));
        }

        /// <summary>
        /// Creates the error response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private Error CreateErrorResponse(string message)
        {
            return CreateErrorResponse(GenerateErrorCode(_options.ErrorCodePrefix), message);
        }

        /// <summary>
        /// Creates the error response.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private Error CreateErrorResponse(string errorCode, string message)
        {
            return new Error
            {
                ErrorCode = errorCode,
                Message = message
            };
        }

        /// <summary>
        /// Generates the error code.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        private string GenerateErrorCode(string prefix)
        {
            var uniqueValue = Guid.NewGuid()
                .ToString()
                .Replace("-", string.Empty)
                .Substring(0, 8)
                .ToUpper();

            return $"{prefix}{uniqueValue}";
        }

        /// <summary>
        /// Clears the cache headers.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        private Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.FromResult(response);
        }
    }
}