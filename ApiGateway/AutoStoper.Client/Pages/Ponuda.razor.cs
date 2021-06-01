using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Pages
{
    public partial class Ponuda
    {
        public Dictionary<int, string> collection { get; set; }
        public int activeKey { get; set; }
        public DateTime? date { get; set; }
        public TimeSpan? time { get; set; }
        protected override async Task OnInitializedAsync()
        {

            collection = new();
            collection.Add(1, "Datum");
            collection.Add(2, "Polazište");
            collection.Add(3, "Odredište");
            collection.Add(4, "Detalji");

            activeKey = 1;
        }

        public async Task SetPreviousItemActive()
        {
            if (activeKey <= 1)
            {
                activeKey = 4;
            }
            else
            {
                activeKey--;
            }

            StateHasChanged();
        }

        public async Task SetNextItemActive()
        {
            if (activeKey >= 4)
            {
                activeKey = 1;
            }
            else
            {
                activeKey++;
            }

            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (activeKey == 2)
            {
                await InicijalizirajMapuPolaziste();
            }

            if (activeKey == 3)
            {
                await InicijalizirajMapuOdrediste();
            }
        }

        public async Task InicijalizirajMapuPolaziste()
        {
            await _jsRuntime.InvokeVoidAsync("inicijalizirajMapuPolaziste");
        }

        public async Task InicijalizirajMapuOdrediste()
        {
            await _jsRuntime.InvokeVoidAsync("inicijalizirajMapuOdrediste");
        }
    }
}
