using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RM.CommonLibrary.Interfaces
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
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T content,bool isBatchJob=false);

        /// <summary>
        /// Invoke web api method and put content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Web api URl</param>
        /// <param name="content">Arguments</param>
        /// <returns></returns>
        Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T content, bool isBatchJob = false);

        /// <summary>
        /// Invoke web api get method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Web api URl</param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(string url, bool isBatchJob = false);

        /// <summary>
        /// Set Web api URL
        /// </summary>
        /// <param name="addr"></param>
        void SetBaseAddress(Uri address);

        /// <summary>
        /// Invoke web api delete method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Web api URl</param>
        /// <returns></returns>
        Task<HttpResponseMessage> DeleteAsync(string url, bool isBatchJob = false);
    }
}