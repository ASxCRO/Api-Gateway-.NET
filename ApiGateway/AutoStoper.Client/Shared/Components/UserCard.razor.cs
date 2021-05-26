using Microsoft.AspNetCore.Components;
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
            var user = _authorizationService.User;

            this.Email = user.Username;
            this.FirstName = user.FirstName;
            this.SecondName = user.LastName;
            if (this.FirstName.Length > 0)
            {
                FirstLetterOfName = FirstName[0];
            }
            ImageDataUrl = "https://media.istockphoto.com/photos/portrait-of-smiling-mature-man-standing-on-white-background-picture-id1245820678?s=612x612";
        }
    }
}