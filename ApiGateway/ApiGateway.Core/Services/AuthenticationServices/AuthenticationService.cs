using ApiGateway.Core.AuthenticationServices;
using ApiGateway.Core.HttpServices;
using ApiGateway.Core.LocalStorageServices;
using ApiGateway.Core.Models.Enums;
using ApiGateway.Core.Models.RequestModels;
using ApiGateway.Core.RequestModels;
using ApiGateway.Core.ResponseModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Core.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IWebAssemblyHttpService webAssemblyHttpService;
        private readonly NavigationManager nvgMgr;
        private readonly UserService _userService;
        private readonly ILocalStorageService _localStorageService;
        public LoginResponse User { get; set; }

        public AuthenticationService(NavigationManager nvgMgr, ILocalStorageService localStorageService, IWebAssemblyHttpService webAssemblyHttpService)
        {
            this.nvgMgr = nvgMgr;
            this._localStorageService = localStorageService;
            this.webAssemblyHttpService = webAssemblyHttpService;
            this._userService = new UserService(localStorageService);
        }

        public async Task Initialize()
        {
            User = await _userService.GetCurrentUser();
        }

        public async Task<bool> Login(LoginRequest loginRequest)
        {
            var response = await webAssemblyHttpService.Fetch<LoginResponse>(Client.ApiGateway, loginRequest, HttpMethod.Post, "/authenticate");
            
            if(response is not null)
            {
                User = response;
                await _localStorageService.SetItem("user", User);
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem("user");
            nvgMgr.NavigateTo("/",true);
        }

        public async Task<bool> Register(RegisterRequest requestModel)
        {

            var response = await webAssemblyHttpService.Fetch<LoginResponse>(Client.ApiGateway, requestModel, HttpMethod.Post, "/register");

            if (response is not null)
            {
                this.User = response;
                await _localStorageService.SetItem("user", User);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> Update(User.User user)
        {
            var response = await webAssemblyHttpService.Fetch<LoginResponse>(Client.ApiGateway, user, HttpMethod.Post, "/azurirajkorisnika");
            return response is not null ? true : false;
        }

        public async Task<User.User> GetById(int id)
        {
            var response = await webAssemblyHttpService.Fetch<ApiGateway.Core.User.User>(Client.ApiGateway, null, HttpMethod.Get, $"/korisnik?id={id}");
            return response is not null ? response : null;
        }
    }
}
