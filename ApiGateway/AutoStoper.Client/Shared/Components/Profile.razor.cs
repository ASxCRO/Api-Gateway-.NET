using ApiGateway.Core.User;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared.Components
{
    public partial class Profile
    {
        private char FirstLetterOfName { get; set; }

        private readonly User profileModel = new User { FirstName = "Antonio", LastName= "Supan", Email = "antonio.suusp@gmail.com"};
        public string UserId { get; set; }

        private async Task UpdateProfileAsync()
        {
            //var response = await _accountManager.UpdateProfileAsync(profileModel);
            if (true)
            {
                //await _authenticationManager.Logout();
                _snackBar.Add("Your Profile has been updated. Please Login to Continue.", Severity.Success);
                _navigationManager.NavigateTo("/");
            }
            else
            {
                foreach (var message in new List<string> { "Greška prilikom update profila"})
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
        }


        private async Task DeleteAsync()
        {
            //var parameters = new DialogParameters
            //{
            //    //TODO: localize
            //    {"ContentText", $"Do you want to delete the profile picture of {profileModel.Email} ?"}
            //};
            //var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            //var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Delete", parameters, options);
            //var result = await dialog.Result;
            //if (!result.Cancelled)
            //{
            //    var request = new UpdateProfilePictureRequest() { Data = null, FileName = string.Empty, UploadType = Application.Enums.UploadType.ProfilePicture };
            //    var data = await _accountManager.UpdateProfilePictureAsync(request, UserId);
            //    if (data.Succeeded)
            //    {
            //        await _localStorage.RemoveItemAsync("userImageURL");
            //        ImageDataUrl = string.Empty;
            //        _navigationManager.NavigateTo("/account", true);
            //    }
            //    else
            //    {
            //        foreach (var error in data.Messages)
            //        {
            //            _snackBar.Add(localizer[error], Severity.Error);
            //        }
            //    }
            //}
        }
    }
}
