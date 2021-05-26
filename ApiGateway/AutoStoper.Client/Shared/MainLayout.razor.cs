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
        private string CurrentUserId { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }

        private async Task LoadDataAsync()
        {
            var user = _authorizationService.User;
            if (user == null) return;
            CurrentUserId = user.Id;
            this.FirstName = user.FirstName;
            if (this.FirstName.Length > 0)
            {
                FirstLetterOfName = FirstName[0];
            }
            this.SecondName = user.LastName;
            this.Email = user.Username;
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
