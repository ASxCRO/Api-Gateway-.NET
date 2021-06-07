using ApiGateway.Core.User;
using AutoStoper.Client.Shared.Preferences;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared
{
    public partial class MainLayout
    {
        private int CurrentUserId { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }
        private string ImageDataUrl { get; set; }

        private async Task LoadDataAsync()
        {
            var user = new User();
            if (_authorizationService.User is not null)
                user = await _authorizationService.GetById(_authorizationService.User.Id);
            else
                user = null;

            if (user == null) return;
            CurrentUserId = user.Id;
            this.FirstName = user.FirstName;
            if (this.FirstName.Length > 0)
            {
                FirstLetterOfName = FirstName[0];
            }
            this.SecondName = user.LastName;
            this.Email = user.Username;
            if(user.Image is not null)
            {
                var base64 = Convert.ToBase64String(user.Image);
                ImageDataUrl = String.Format("data:image/gif;base64,{0}", base64);
            }
        }

        private MudTheme _currentTheme;
        private bool _drawerOpen = true;

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = BlazorHeroTheme.DefaultTheme;
            await LoadDataAsync();
           // _currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();
        }


        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        private async Task DarkMode()
        {
            bool isDarkMode = false;
            _currentTheme = isDarkMode
                ? BlazorHeroTheme.DefaultTheme
                : BlazorHeroTheme.DarkTheme;
        }

        public void Dispose()
        {
           // _interceptor.DisposeEvent();
            //_ = hubConnection.DisposeAsync();
        }
    }
}
