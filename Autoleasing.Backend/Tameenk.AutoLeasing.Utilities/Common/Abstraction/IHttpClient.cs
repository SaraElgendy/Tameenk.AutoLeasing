using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tameenk.AutoLeasing.Utilities
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> headers = null);

        Task<string> GetStringAsync(string uri, bool returnResponseOnFailure = false, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> headers = null);

        Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer", Dictionary<string, string> headers = null);

        Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer");
        string GetUser();
        HttpContext GetCurrentContext();
    }
}
