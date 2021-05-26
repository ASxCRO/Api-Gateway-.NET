using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Pages.Authentication
{
    public partial class Logout
    {
        protected override async Task OnInitializedAsync()
        {
           await _authorizationService.Logout();
        }
    }
}
