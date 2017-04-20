using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fmo.NYBLoader.Interfaces;

namespace Fmo.NYBLoader.Common
{
    /// <summary>
    /// Handle web api calls from file loader and receiver assembly
    /// </summary>
    public class HttpHandler : IHttpHandler
    {
        private HttpClient client;

        public HttpHandler()
        {
            if (this.client == null)
            {
                client = new HttpClient();
            }
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
            return await client.PostAsJsonAsync<T>(url, content);
        }

    }
}
