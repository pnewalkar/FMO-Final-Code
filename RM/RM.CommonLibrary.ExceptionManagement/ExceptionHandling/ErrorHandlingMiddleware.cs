namespace RM.CommonLibrary.ExceptionManagement.ExceptionHandling
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.Net.Http.Headers;
    using Newtonsoft.Json;
    using RM.CommonLibrary.ExceptionManagement.ExceptionHandling.ResponseExceptionHandler;
    using RM.CommonLibrary.ExceptionMiddleware;
    using RM.CommonLibrary.HelperMiddleware;
    using RM.CommonLibrary.LoggingMiddleware;

    /// <summary>
    /// Error Handling Middleware. Interceptor for handling all the errors in controllers.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ResponseExceptionHandlerOptions options;
        private readonly Func<object, Task> clearCacheHeadersDelegate;
        private IExceptionHelper exceptionHelper;
        private ILoggingHelper loggingHelper;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILoggingHelper loggingHelper,
            IExceptionHelper exceptionHelper,
            IOptions<ResponseExceptionHandlerOptions> options)
        {
            this.next = next;
            this.exceptionHelper = exceptionHelper;
            this.loggingHelper = loggingHelper;
            this.options = options.Value;
            this.clearCacheHeadersDelegate = this.ClearCacheHeaders;
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
                await this.next(context);
            }
            catch (Exception ex)
            {
                this.exceptionHelper.HandleException(ex, ExceptionHandlingPolicy.LogAndWrap);
                if (ex.InnerException == null)
                {
                    await this.WriteExceptionToContextAsync(context, ex);
                }
                else
                {
                    await this.WriteExceptionToContextAsync(context, ex.InnerException);
                }
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
                if (this.options.Responses.TryGetValue(exception.GetType(), out exceptionResponse))
                {
                    context.Response.StatusCode = exceptionResponse.StatusCode; // Replace default status code with actual.
                    response = this.GetExceptionResponse(exception, exceptionResponse);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = this.CreateErrorResponse(this.options.DefaultErrorMessage);
                }

                var result = JsonConvert.SerializeObject(response, this.options.SerializerSettings);
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(result);
            }
            catch (Exception)
            {
                this.loggingHelper.Log(ErrorConstants.Err_ExecutingErrorHandler, TraceEventType.Error, exception);
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
                return this.CreateErrorResponse(exceptionResponse.Message);
            }
            else if (exceptionResponse.StatusCode.Equals((int)HttpStatusCode.InternalServerError) && ex.Data.Count.Equals(0))
            {
                return this.CreateErrorResponse(this.options.DefaultErrorMessage);
            }

            if (ex.Data.Count > 0)
            {
                string userFriendlyMessage = ex.Data["userFriendlyMessage"].ToString();
                return exceptionResponse.Response ?? this.CreateErrorResponse(userFriendlyMessage.Replace(Environment.NewLine, " "));
            }

            return exceptionResponse.Response ?? this.CreateErrorResponse(ex.Message.Replace(Environment.NewLine, " "));
        }

        /// <summary>
        /// Creates the error response.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private Error CreateErrorResponse(string message)
        {
            return new Error
            {
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