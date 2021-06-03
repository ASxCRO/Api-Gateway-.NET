using ApiGateway.Core.Exception;
using ApiGateway.Core.SpinnerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using MudBlazor;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGateway.Core.HttpServices
{
    public class WebassemblyHttpService : IWebAssemblyHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NavigationManager _navigationManager;
        private readonly SpinnerService _spinnerService;
        private readonly ISnackbar _snackbarService;

        public WebassemblyHttpService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager, SpinnerService spinnerService, ISnackbar snackbarService)
        {
            _httpClientFactory = httpClientFactory;
            _navigationManager = navigationManager;
            _spinnerService = spinnerService;
            _snackbarService = snackbarService;
        }

        public HttpRequestMessage ForgeRequest(object requestModel, HttpMethod httpMethod, string uri)
        {
            _spinnerService.Show();
            var request = new HttpRequestMessage(httpMethod, uri);
            if (httpMethod != HttpMethod.Get)
                request.Content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
            request.SetBrowserRequestMode(BrowserRequestMode.NoCors);
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return request;
        }
        public async Task<T> SendRequest<T>(string clientName, HttpRequestMessage request, string token = null)
        {
            using var client = _httpClientFactory.CreateClient(clientName);
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            HttpResponseMessage response = new HttpResponseMessage(); 
            if (request.Method == HttpMethod.Post)
            {
                response = await client.PostAsync(request.RequestUri.OriginalString, request.Content);
            }
            else if (request.Method == HttpMethod.Get)
            {
                response = await client.GetAsync(request.RequestUri.OriginalString);
            }

            _spinnerService.Hide();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigationManager.NavigateTo("/logout");
                return default;
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                _snackbarService.Add("Niste u mogućnosti izvršiti akciju", Severity.Error);
                return default;
            }

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                var exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomExceptionMsg>(msg);
                if (exceptionMsg != null)
                    _snackbarService.Add(exceptionMsg.Detail, Severity.Error);
                return default;
            }

            return await response?.Content?.ReadFromJsonAsync<T>();
        }

        public async Task<bool> Send(string clientName, object requestModel, HttpMethod httpMethod, string uri, string token = null)
        {
            var request = ForgeRequest(requestModel, httpMethod, uri);
            using (var client = _httpClientFactory.CreateClient(clientName))
            {
                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.SendAsync(request);

                _spinnerService.Hide();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _navigationManager.NavigateTo("/logout");
                    return false;
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _snackbarService.Add("Niste u mogućnosti izvršiti akciju", Severity.Error);
                    return default;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var exceptionMsg = await response.Content.ReadFromJsonAsync<CustomExceptionMsg>();
                    //var exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<ExceptionMsg>(msg);
                    if (exceptionMsg != null)
                        _snackbarService.Add(exceptionMsg.Detail, Severity.Error);
                    else
                        _snackbarService.Add("Dogodila se pogreška", Severity.Error);
                    return false;
                }
            }
            return true;
        }

        public async Task<T> Fetch<T>(string clientName, object requestModel, HttpMethod httpMethod, string uri, string token)
        {
            var request = ForgeRequest(requestModel, httpMethod, uri);
            return await SendRequest<T>(clientName, request, token);
        }
    }
}
