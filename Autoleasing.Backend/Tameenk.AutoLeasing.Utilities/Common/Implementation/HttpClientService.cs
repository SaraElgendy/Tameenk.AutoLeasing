using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Tameenk.AutoLeasing.Utilities
{
    public class HttpClientService : IHttpClient
    {

        private HttpClient client;
        private readonly HttpContextAccessor httpContextAccessor;       

        public HttpClientService()
        {
            client = new HttpClient();
            this.httpContextAccessor = new HttpContextAccessor();
        }


        public string GetUser()
        {
            return httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value;
        }

        public async Task<string> GetStringAsync(string uri, bool returnResponseOnFailure = false, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            SetAuthorizationHeader(requestMessage);

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            if (headers != null)
            {
                AddHeadersToRequest(requestMessage, headers);
            }
            var response = await client.SendAsync(requestMessage).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (returnResponseOnFailure)
                {
                    throw new HttpRequestException(responseString);
                }
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public Task<HttpResponseMessage> GetAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer", Dictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            SetAuthorizationHeader(requestMessage);
            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }
            if (headers != null)
            {
                AddHeadersToRequest(requestMessage, headers);
            }
            return client.SendAsync(requestMessage);
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer", Dictionary<string, string> headers = null)
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            // a new StringContent must be created for each retry
            // as it is disposed after each call

            var requestMessage = new HttpRequestMessage(method, uri);

            SetAuthorizationHeader(requestMessage);
            if (item is HttpContent)
            {
                requestMessage.Content = item as HttpContent;
            }
            else
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");
            }



            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            if (headers != null)
            {
                AddHeadersToRequest(requestMessage, headers);
            }

            if (requestId != null)
            {
                requestMessage.Headers.Add("x-requestid", requestId);
            }

            var response = await client.SendAsync(requestMessage).ConfigureAwait(false);

            // raise exception if HttpResponseCode 500
            // needed for circuit breaker to track fails

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }

            return response;
        }


        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer", Dictionary<string, string> headers = null)
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, item, authorizationToken, requestId, authorizationMethod, headers).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, item, authorizationToken, requestId, authorizationMethod);
        }
        public async Task<HttpResponseMessage> DeleteAsync(string uri, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            SetAuthorizationHeader(requestMessage);

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            if (requestId != null)
            {
                requestMessage.Headers.Add("x-requestid", requestId);
            }

            return await client.SendAsync(requestMessage).ConfigureAwait(false);
        }

        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
        }

        private void AddHeadersToRequest(HttpRequestMessage requestMessage, Dictionary<string, string> headers)
        {
            if (headers != null && headers.Count > 0)
            {
                foreach (var item in headers)
                {
                    requestMessage.Headers.Add(item.Key, item.Value);
                }
            }
        }

        public HttpContext GetCurrentContext()
        {
            return this.httpContextAccessor.HttpContext;
        }
    }
}
