using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Shared.Preferences
{
    public interface IPreferenceManager
    {
        Task SetPreference(ClientPreference preference);

        Task<ClientPreference> GetPreference();
    }
}
