using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using ApiGateway.Core.RequestModels;

namespace AutoStoper.Client.Pages.Authentication
{
    public partial class Login
    {
        private LoginRequest tokenModel = new LoginRequest();

        protected override async Task OnInitializedAsync()
        {
            if(_authorizationService.User is not null)
            {
                _navigationManager.NavigateTo("/index", true);
            }
        }

        private async Task SubmitAsync()
        {
            var result = await _authorizationService.Login(tokenModel);
            if (result)
            {
                _snackBar.Add($"Dobrodošli {tokenModel.Username}.", Severity.Success);
                _navigationManager.NavigateTo("/index", true);
            }
            else
            {
                _snackBar.Add($"Korisničko ime/lozinka nije ispravno.", Severity.Error);

            }
        }

        private bool PasswordVisibility;
        private InputType PasswordInput = InputType.Password;
        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (PasswordVisibility)
            {
                PasswordVisibility = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                PasswordVisibility = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        private void FillBasicUserCredentials()
        {
            tokenModel.Username = "test";
            tokenModel.Password = "test";
        }
    }
}