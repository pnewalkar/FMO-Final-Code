using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.CommonLibrary.HttpHandler
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

        public HttpHandler()
        {
            if (this.client == null)
            {
                client = new HttpClient();
            }

            tokenGenerationURl = ConfigurationManager.AppSettings[Constants.FMOTokenGenerationUrl].ToString();
            userName = ConfigurationManager.AppSettings[Constants.FMOWebAPIUser].ToString();
            serilaize = new JavaScriptSerializer();
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
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T content, bool isBatchJob = false)
        {
            string token = string.Empty;
            if (isBatchJob)
            {
                token = await GetSecurityToken();
            }
            else
            {
                token = await GetSecurityTokenForAPI();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
            return await client.PostAsJsonAsync<T>(url, content);
        }

        /// <summary>
        /// Invoke web api method and put content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Web api URl</param>
        /// <param name="content">Arguments</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T content, bool isBatchJob = false)
        {
            string token = string.Empty;
            if (isBatchJob)
            {
                token = await GetSecurityToken();
            }
            else
            {
                token = await GetSecurityTokenForAPI();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
            return await client.PutAsJsonAsync<T>(url, content);
        }

        /// <summary>
        /// Invoke web api get method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Web api URl</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string url, bool isBatchJob = false)
        {
            string token = string.Empty;
            if (isBatchJob)
            {
                token = await GetSecurityToken();
            }
            else
            {
                token = await GetSecurityTokenForAPI();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
            return await client.GetAsync(url);
        }

        /// <summary>
        /// Invoke web api delete method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Web api URl</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteAsync(string url, bool isBatchJob = false)
        {
            string token = string.Empty;
            if (isBatchJob)
            {
                token = await GetSecurityToken();
            }
            else
            {
                token = await GetSecurityTokenForAPI();
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
            return await client.DeleteAsync(url);
        }

        private async Task<string> GetSecurityToken()
        {
            object objtoken;
            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>(Constants.UserName, userName) });
            var result = await client.PostAsync(tokenGenerationURl, content);
            string token = await result.Content.ReadAsStringAsync();
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

        private async Task<string> GetSecurityTokenForAPI()
        {
            if (string.IsNullOrEmpty(WebHelpers.AuthorizationHeader))
            {
                return await GetSecurityToken();
            }
            else
            {
                return AuthenticationHeaderValue.Parse(WebHelpers.AuthorizationHeader).Parameter;
            }
        }
    }
}