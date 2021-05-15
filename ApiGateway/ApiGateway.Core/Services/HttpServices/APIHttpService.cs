using ApiGateway.Core.Exception;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGateway.Core.HttpServices
{
    public class APIHttpService : IAPIHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APIHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> Fetch(string clientName, object requestModel, HttpMethod httpMethod, string uri, BrowserRequestMode browserRequestMode, BrowserRequestCredentials browserRequestCredentials, string token = null)
        {
            var request = ForgeRequest(requestModel, httpMethod, uri, browserRequestMode, browserRequestCredentials);
            return await SendRequest(clientName, request, token);
        }

        public HttpRequestMessage ForgeRequest(object requestModel, HttpMethod httpMethod, string uri, BrowserRequestMode browserRequestMode, BrowserRequestCredentials browserRequestCredentials)
        {
            var request = new HttpRequestMessage(httpMethod, uri);
            if (httpMethod != HttpMethod.Get)
                request.Content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
            request.SetBrowserRequestMode(browserRequestMode);
            request.SetBrowserRequestCredentials(browserRequestCredentials);

            return request;
        }

        public async Task<HttpResponseMessage> SendRequest(string clientName, HttpRequestMessage request, string token = null)
        {
            using var client = _httpClientFactory.CreateClient(clientName);
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                var exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomExceptionMsg>(msg);
                if (exceptionMsg != null)
                    throw new CustomException(exceptionMsg.Detail);
            }


            return response;
        }
    }
}
