using Microsoft.AspNetCore.Http;

namespace RM.CommonLibrary.HelperMiddleware
{
    public class WebHelpers
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static HttpContext HttpContext
        {
            get { return _httpContextAccessor.HttpContext; }
        }

        public static string GetRemoteIP
        {
            get { return HttpContext.Connection.RemoteIpAddress.ToString(); }
        }

        public static string GetUserAgent
        {
            get { return HttpContext.Request.Headers["User-Agent"].ToString(); }
        }

        public static string GetScheme
        {
            get { return HttpContext.Request.Scheme; }
        }

        public static string AuthorizationHeader
        {
            get
            {
                return HttpContext.Request.Headers["Authorization"].ToString();
            }
        }

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}