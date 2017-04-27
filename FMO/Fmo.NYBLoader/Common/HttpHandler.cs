using System;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using Fmo.NYBLoader.Interfaces;
using Fmo.Common.Interface;
using Fmo.Common.Constants;
using Fmo.Common.LoggingManagement;

namespace Fmo.NYBLoader.Common
{
    /// <summary>
    /// Handle web api calls from file loader and receiver assembly
    /// </summary>
    public class HttpHandler : IHttpHandler
    {
        private HttpClient client;
        private string tokenGenerationURl = string.Empty;
        private string userName = string.Empty;
        private JavaScriptSerializer serilaize;
        private LoggingHelper loggingHelper;

        public HttpHandler()
        {
            if (this.client == null)
            {
                client = new HttpClient();
            }
            tokenGenerationURl = ConfigurationManager.AppSettings[Constants.FMOTokenGenerationUrl].ToString();
            userName = ConfigurationManager.AppSettings[Constants.FMOWebAPIUser].ToString();
            serilaize = new JavaScriptSerializer();
            loggingHelper = new LoggingHelper();
        }

        /// <summary>
        /// Set Web api URL
        /// </summary>
        /// <param name="addr"></param>
        public void SetBaseAddress(Uri address)
        {
            this.client.BaseAddress = address;
        }

        /// <summary>
        /// Invoke web api method and post content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Web api URl</param>
        /// <param name="content">Arguments</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T content)
        {
            string token = await GetSecurityToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
            return await client.PostAsJsonAsync<T>(url, content);
        }

        private async Task<string> GetSecurityToken()
        {
            object objtoken;
            string tokenResponse = Constants.TokenResponse;
            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>(Constants.UserName, userName) });
            var result = await client.PostAsync(tokenGenerationURl, content);
            string token = await result.Content.ReadAsStringAsync();
            loggingHelper.LogInfo(tokenResponse + token);
            if (!string.IsNullOrEmpty(token))
            {
                Dictionary<string, object> response = serilaize.Deserialize<Dictionary<string, object>>(token);
                response.TryGetValue(Constants.AccessToken, out objtoken);
                return objtoken.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
