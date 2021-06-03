using ApiGateway.Core.Models.RequestModels;
using MudBlazor;
using System.Threading.Tasks;

namespace AutoStoper.Client.Pages.Authentication
{
    public partial class Register
    {
        private RegisterRequest registerUserModel = new RegisterRequest();

        private async Task SubmitAsync()
        {
            var response = await _authorizationService.Register(registerUserModel);
            if(response)
            {
                _navigationManager.NavigateTo("/index");
            }
            else
            {
                _snackBar.Add("Registracija nije prosla", Severity.Error);
            }
        }

        private bool PasswordVisibility;
        private InputType PasswordInput = InputType.Password;
        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
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
    }
}
