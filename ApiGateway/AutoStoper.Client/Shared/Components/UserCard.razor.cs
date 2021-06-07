using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared
{
    public partial class UserCard
    {
        [Parameter] public string Class { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }

        [Parameter]
        public string ImageDataUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var user = await _authorizationService.GetById(_authorizationService.User.Id);

            this.Email = user.Username;
            this.FirstName = user.FirstName;
            this.SecondName = user.LastName;
            if (this.FirstName.Length > 0)
            {
                FirstLetterOfName = FirstName[0];
            }
            if (user.Image is not null)
            {
                var base64 = Convert.ToBase64String(user.Image);
                ImageDataUrl = String.Format("data:image/gif;base64,{0}", base64);
            }
        }
    }
}