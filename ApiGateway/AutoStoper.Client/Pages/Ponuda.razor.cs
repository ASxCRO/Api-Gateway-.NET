using ApiGateway.Core.Models;
using ApiGateway.Core.Models.ResponseModels;
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
        public bool KucniLjubimci { get; set; }
        public bool Pusenje { get; set; }
        public bool Glazba { get; set; }
        public bool Razgovor { get; set; }
        public int LjudiUAutu { get; set; }

        public Lokacija lokacijaPolaziste { get; set; }
        public Lokacija lokacijaOdrediste { get; set; }
        public Ruta Ruta { get; set; }
        private DotNetObjectReference<Ponuda> objRef { get; set; }

        protected override async Task OnInitializedAsync()
        {
            collection = new();
            collection.Add(1, "Datum");
            collection.Add(2, "Polazište");
            collection.Add(3, "Odredište");
            collection.Add(4, "Detalji");

            activeKey = 1;

            objRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("GLOBAL.SetDotnetReference", objRef);
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
            switch (activeKey)
            {
                case 2:
                    await InicijalizirajMapuPolaziste();
                    break;
                case 3:
                    await InicijalizirajMapuOdrediste();
                    break;
                case 4:
                    if(lokacijaPolaziste is null)
                    {
                        await InicijalizirajMapuRuta();
                        await GetLatLngPolazisteOdrediste();

                    }
                    break;
                default:
                    break;
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

        public async Task InicijalizirajMapuRuta()
        {
            await _jsRuntime.InvokeVoidAsync("inicijalizirajMapuRuta");
        }

        public async Task GetLatLngPolazisteOdrediste()
        {
            lokacijaPolaziste = await _jsRuntime.InvokeAsync<Lokacija>("GetLokacijaPolaziste");
            lokacijaOdrediste = await _jsRuntime.InvokeAsync<Lokacija>("GetLokacijaOdrediste");
        }

        [JSInvokable]
        public async Task ShowMoreInfo()
        {
            Ruta = await _jsRuntime.InvokeAsync<Ruta>("GetRutu");
            Ruta.Distanca = Math.Round(Ruta.Distanca / 1000,2);
            Ruta.Vrijeme = Math.Round( Ruta.Vrijeme / 60,2);
            StateHasChanged();
        }

        public async Task ObjaviPrijevoz()
        {
            var voznja = new Voznja { 
                Adresa = new Adresa
                {
                    Distanca = Ruta.Distanca,
                    Vrijeme = Ruta.Vrijeme,
                    LatPolaziste = lokacijaPolaziste.Lat,
                    LngPolaziste = lokacijaPolaziste.Lng,
                    LatOdrediste = lokacijaOdrediste.Lat,
                    LngOdrediste = lokacijaOdrediste.Lng,
                    Polaziste = Ruta.Polaziste,
                    Odrediste = Ruta.Odrediste
                },
                LjubimciDozvoljeni = KucniLjubimci,
                MaksimalnoPutnika = LjudiUAutu,
                AutomatskoOdobrenje = Razgovor,
                DateTime = date.Value.AddHours(time.Value.Hours).AddMinutes(time.Value.Minutes),
                PusenjeDozvoljeno = Pusenje
            };
            await _autoStoperService.Insert(voznja);
            _snackBar.Add("Uspješno ste unjeli vožnju!", Severity.Success);
            await Task.Delay(500);
            _navigationManager.NavigateTo("/index");
        }
    }
}
