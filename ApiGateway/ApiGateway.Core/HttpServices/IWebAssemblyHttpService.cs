using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Core.HttpServices
{
    public interface IWebAssemblyHttpService
    {
        public HttpRequestMessage ForgeRequest(object requestModel, HttpMethod httpMethod, string uri, BrowserRequestMode browserRequestMode, BrowserRequestCredentials browserRequestCredentials);
        public Task<T> SendRequest<T>(string clientName, HttpRequestMessage request, string token = null);
        Task<bool> Send(string clientName, object requestModel, HttpMethod httpMethod, string uri, BrowserRequestMode browserRequestMode, BrowserRequestCredentials browserRequestCredentials, string token = null);
        Task<T> Fetch<T>(string clientName, object requestModel, HttpMethod httpMethod, string uri, BrowserRequestMode browserRequestMode, BrowserRequestCredentials browserRequestCredentials, string token = null);
    }
}