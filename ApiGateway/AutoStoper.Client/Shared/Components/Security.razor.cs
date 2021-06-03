using ApiGateway.Core.User;
using MudBlazor;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared.Components
{
    public partial class Security
    {
        public User profileModel { get; set; }
        public string newPassword { get; set; }
        public string newPassword2 { get; set; }
        public string todayPassword { get; set; }

        protected override async Task OnInitializedAsync()
        {
            profileModel = new();
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            profileModel = await _authorizationService.GetById(_authorizationService.User.Id);
        }

        public async Task ChangePasswordAsync()
        {
            if(newPassword != newPassword2)
                _snackBar.Add("Lozinke se ne podudaraju", Severity.Error);
            else if(todayPassword != profileModel.Password)
                _snackBar.Add("Sadašnja lozinka nije ispravna", Severity.Error);
            else
            {
                profileModel.Password = newPassword;
                var response = await _authorizationService.Update(profileModel);
                if (response)
                    _snackBar.Add("Lozinka je promjenjena", Severity.Success);
                else
                    _snackBar.Add("Pogreška pri promjeni lozinke", Severity.Error);
            }
        }
    }
}
