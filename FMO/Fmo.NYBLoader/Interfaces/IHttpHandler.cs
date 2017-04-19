using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Interfaces
{
    /// <summary>
    /// Interface to handle web api calls from file loader and receiver assembly
    /// </summary>
    public interface IHttpHandler
    {
        /// <summary>
        /// Invoke web api method and post content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Web api URl</param>
        /// <param name="content">Arguments</param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T content);


        /// <summary>
        /// Set Web api URL
        /// </summary>
        /// <param name="addr"></param>
        void SetBaseAddress(Uri address);
    }

}
