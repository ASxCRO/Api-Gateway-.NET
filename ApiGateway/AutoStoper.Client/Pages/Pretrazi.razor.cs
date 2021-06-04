using ApiGateway.Core.Models;
using ApiGateway.Core.Models.ResponseModels;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.Pages
{
    public partial class Pretrazi
    {
        public Dictionary<int,string> collection { get; set; }
        public int activeKey { get; set; }
        public DateTime? date { get; set; }
        public Lokacija lokacijaPolaziste { get; set; }
        public Lokacija lokacijaOdrediste { get; set; }
        public List<Voznja> Voznje { get; set; }

        protected override async Task OnInitializedAsync()
        {
            collection = new();
            collection.Add(1, "Datum");
            collection.Add(2, "Polazište");
            collection.Add(3, "Odredište");
            collection.Add(4, "Odaberi prijevoz");

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

        public async Task GetLatLngPolazisteOdrediste()
        {
            lokacijaPolaziste = await _jsRuntime.InvokeAsync<Lokacija>("GetLokacijaPolaziste");
            lokacijaOdrediste = await _jsRuntime.InvokeAsync<Lokacija>("GetLokacijaOdrediste");
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

            if (activeKey == 4)
            {
                if(Voznje is null)
                {
                    Voznje = new();
                    await GetLatLngPolazisteOdrediste();
                    await DohvatiVoznjeUDosegu();
                }
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

        public async Task DohvatiVoznjeUDosegu()
        {
            var sveVoznje = await _autoStoperService.GetAll();
            List<Lokacija> sveLokacijePolazista = new(), sveLokacijeOdredista = new();


            foreach (var item in sveVoznje)
            {
                sveLokacijePolazista.Add(new Lokacija
                {
                    Lat = item.Adresa.LatPolaziste,
                    Lng = item.Adresa.LngPolaziste
                });

                sveLokacijeOdredista.Add(new Lokacija
                {
                    Lat = item.Adresa.LatOdrediste,
                    Lng = item.Adresa.LngOdrediste
                });
            }

            var lokacijeKojePasuPolazistu = await _jsRuntime.InvokeAsync<List<Lokacija>>("dohvatiVoznjeURadiusu", sveLokacijePolazista, lokacijaPolaziste);
            var lokacijeKojePasuOdredistu =
            await _jsRuntime.InvokeAsync<List<Lokacija>>("dohvatiVoznjeURadiusu", sveLokacijeOdredista, lokacijaOdrediste);

            foreach (var voznja in sveVoznje)
                foreach (var lokacijaPolaziste in lokacijeKojePasuPolazistu)
                    foreach (var lokacijaOdrediste in lokacijeKojePasuOdredistu)
                            if (lokacijaPolaziste.Lat == voznja.Adresa.LatPolaziste &&
                                lokacijaPolaziste.Lng == voznja.Adresa.LngPolaziste &&
                                lokacijaOdrediste.Lat == voznja.Adresa.LatOdrediste &&
                                lokacijaOdrediste.Lng == voznja.Adresa.LngOdrediste)
                                    Voznje.Add(voznja);


            StateHasChanged();
        }
    }
}
