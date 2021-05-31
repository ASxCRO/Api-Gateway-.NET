using ApiGateway.Core.User;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared.Components
{
    public partial class Security
    {
        public readonly User passwordModel = new User { FirstName = "Antonio", LastName = "Supan", Email = "antonio.suups@gmail.com", Password ="test"};

        public async Task ChangePasswordAsync()
        {
            //var response = await _accountManager.ChangePasswordAsync(passwordModel);
            if (true)
            {
                _snackBar.Add("Lozinka je promjenjena", Severity.Success);
                passwordModel.Password = string.Empty;
            }
        }
    }
}
