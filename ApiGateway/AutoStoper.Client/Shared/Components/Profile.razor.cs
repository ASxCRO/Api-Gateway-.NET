using ApiGateway.Core.User;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared.Components
{
    public partial class Profile
    {
        private User profileModel { get; set; }
        private string profileImageUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            profileModel = new();
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            profileModel = await _authorizationService.GetById(_authorizationService.User.Id);
            if(profileModel.Image is not null)
            {
                var base64 = Convert.ToBase64String(profileModel.Image);
                profileImageUrl = String.Format("data:image/gif;base64,{0}", base64);
            }
        }

        private async Task UpdateProfileAsync()
        {
            var response =  await _authorizationService.Update(profileModel);
            if (response)
            {
                await _authorizationService.Logout();
                _snackBar.Add("Profil uspješno ažuriran", Severity.Success);
                _navigationManager.NavigateTo("/");
            }
            else
            {
                _snackBar.Add("Nemoguće ažurirati profil", Severity.Error);
            }
        }

        public async Task LoadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles(1))
            {
                var fileByteArray = await ReadFully(file.OpenReadStream());
                var base64 = Convert.ToBase64String(fileByteArray);
                profileImageUrl = String.Format("data:image/gif;base64,{0}", base64);
                profileModel.Image = fileByteArray;
            }
        }

        public static async Task<byte[]> ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await input.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
