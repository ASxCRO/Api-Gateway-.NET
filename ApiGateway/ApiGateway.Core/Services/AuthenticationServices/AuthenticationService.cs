using ApiGateway.Core.AuthenticationServices;
using ApiGateway.Core.HttpServices;
using ApiGateway.Core.LocalStorageServices;
using ApiGateway.Core.RequestModels;
using ApiGateway.Core.ResponseModels;
using ApiGateway.Core.StateServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGateway.Core.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IWebAssemblyHttpService webAssemblyHttpService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NavigationManager nvgMgr;
        private readonly ApiGateway.Core.LocalStorageServices.UserService _userService;
        private readonly ILocalStorageService _localStorageService;
        public LoginResponse LoginResponse { get; set; }
        static HttpClient httpClient { get; set;  }

        public AuthenticationService(NavigationManager nvgMgr, ILocalStorageService localStorageService, IWebAssemblyHttpService webAssemblyHttpService, IHttpClientFactory _httpClientFactory)
        {
            this.nvgMgr = nvgMgr;
            this._localStorageService = localStorageService;
            this.webAssemblyHttpService = webAssemblyHttpService;
            this._httpClientFactory = _httpClientFactory;
            this._userService = new ApiGateway.Core.LocalStorageServices.UserService(localStorageService);
            httpClient = _httpClientFactory.CreateClient("AutoStoper.Gateway");
        }


        public async Task Initialize()
        {
            LoginResponse = await _userService.GetCurrentUser();
        }

        public async Task<bool> Login(LoginRequest loginRequest)
        {
            var requestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{httpClient.BaseAddress}authenticate"),
                Content = new StringContent(JsonSerializer.Serialize(new { loginRequest.Username, loginRequest.Password }), Encoding.UTF8, "application/json"),
            };

            requestMessage.SetBrowserRequestMode(BrowserRequestMode.NoCors);
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await httpClient.PostAsync(requestMessage.RequestUri.OriginalString, requestMessage.Content);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    LoginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
            else
            {
                return false;
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", LoginResponse.Token);
            await _localStorageService.SetItem("user", LoginResponse);
            return true;
        }


        public async Task Logout()
        {
            LoginResponse = null;
            await _localStorageService.RemoveItem("user");
            nvgMgr.NavigateTo("");
        }
    }
}
