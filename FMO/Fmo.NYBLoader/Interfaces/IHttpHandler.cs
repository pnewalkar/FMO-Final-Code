using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Interfaces
{
    public interface IHttpHandler
    { 
         Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T content);

        void SetBaseAddress(Uri addr);
    }

}
