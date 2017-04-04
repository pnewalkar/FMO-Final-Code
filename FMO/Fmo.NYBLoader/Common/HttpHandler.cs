using Fmo.NYBLoader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Common
{
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

        public void SetBaseAddress(Uri addr)
        {
            this.client.BaseAddress = addr;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T content)
        {
            return await client.PostAsJsonAsync<T>(url, content);
        }

    }
}
