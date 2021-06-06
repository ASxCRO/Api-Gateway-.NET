using ApiGateway.Core.Models;
using ApiGateway.Core.Models.RequestModels;
using ApiGateway.Core.Models.ResponseModels;
using ApiGateway.Core.User;
using AutoStoper.Client.ViewModels;
using Microsoft.AspNetCore.Components;
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
        [Inject] private IDialogService DialogService { get; set; }
        public Dictionary<int, string> collection { get; set; }
        public int activeKey { get; set; }
        public DateTime? date { get; set; }
        public TimeSpan? time { get; set; }
        public bool KucniLjubimci { get; set; }
        public bool Pusenje { get; set; }
        public bool Glazba { get; set; }
        public bool AutomatskoOdobrenje { get; set; } = true;
        public int LjudiUAutu { get; set; } = 2;
        public double Cijena { get; set; } = 50;

        public Lokacija lokacijaPolaziste { get; set; }
        public Lokacija lokacijaOdrediste { get; set; }
        public Ruta Ruta { get; set; }
        private DotNetObjectReference<Ponuda> objRef { get; set; }
        public List<VoznjaViewModel> VoznjeTrenutnogKorisnika { get; set; }
        public bool CantGoBackOnlyReset { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            VoznjeTrenutnogKorisnika = new();

            collection = new();
            collection.Add(1, "Datum");
            collection.Add(2, "Polazište");
            collection.Add(3, "Odredište");
            collection.Add(4, "Detalji");

            activeKey = 1;

            objRef = DotNetObjectReference.Create(this);

            await _jsRuntime.InvokeVoidAsync("GLOBAL.SetDotnetReference", objRef);
            await _jsRuntime.InvokeVoidAsync("ResetirajLokacije");

        }

        public async Task SetPreviousItemActive()
        {
            if (activeKey <= 1)
                if (lokacijaPolaziste is null || lokacijaOdrediste is null || !date.HasValue || !time.HasValue)
                    _snackBar.Add("Prvo ispunite sve ostale podatke", Severity.Error);
                else
                    activeKey = 4;
            else
                activeKey--;

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
                await GetLatLngPolazisteOdrediste();
                if (activeKey==3 &&  (lokacijaPolaziste is null || lokacijaOdrediste is null || !date.HasValue || !time.HasValue))
                    _snackBar.Add("Prvo ispunite sve ostale podatke", Severity.Error);
                else
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
                    if(Ruta is null)
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
            CantGoBackOnlyReset = true;
            StateHasChanged();
        }

        public async Task ObjaviPrijevoz()
        {
            bool isValid = true;

            if(this.Cijena <=4.99 || this.Cijena > 1000)
            {
                _snackBar.Add("Cijena mora biti između 5 i 1000");
                isValid = false;
            }
            if (this.LjudiUAutu < 1 || this.LjudiUAutu > 4)
            {
                _snackBar.Add("Možete voziti minimalno jednog a maksimalno četvero ljudi u autu");
                isValid = false;
            }

            if(isValid)
            {
                var voznja = new Voznja
                {
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
                    AutomatskoOdobrenje = this.AutomatskoOdobrenje,
                    DateTime = date.Value.AddHours(time.Value.Hours).AddMinutes(time.Value.Minutes),
                    PusenjeDozvoljeno = Pusenje,
                    Putnici = new List<VoznjaUser>
                {
                    new VoznjaUser
                    {
                        UserId = _authorizationService.User.Id,
                        Vozac = true
                    }
                },
                    Cijena = Math.Round(this.Cijena, 2)
                };
                await _autoStoperService.Insert(voznja);
                _snackBar.Add("Uspješno ste unjeli vožnju!", Severity.Success);
                await Task.Delay(500);
                _navigationManager.NavigateTo("/index");
            }
        }

        public async Task DohvatiVoznjeTrenutnogKorisnika()
        {
            var sveVoznjeTrenutnogKorisnika = await _autoStoperService.GetByUserId(_authorizationService.User.Id);
            VoznjeTrenutnogKorisnika.Clear();
            foreach (var item in sveVoznjeTrenutnogKorisnika)
                foreach (var putnik in item.Putnici.Where(p => p.Vozac))
                    VoznjeTrenutnogKorisnika.Add(new VoznjaViewModel
                    {
                        Voznja = item,
                        Putnici = await GetPutniciFromVoznja(item.Putnici)
                    });

            StateHasChanged();
        }


        private async Task<List<User>> GetPutniciFromVoznja(List<VoznjaUser> putniciIzVoznje)
        {
            var putnici = new List<User>();
            foreach (var item in putniciIzVoznje)
            {
                var currentPutnik = await _authorizationService.GetById(item.UserId);
                putnici.Add(currentPutnik);
            }

            return putnici;
        }

        public async Task PotvrdiOdabir(Voznja voznja)
        {
            bool? result = await DialogService.ShowMessageBox(
                "Potvrda",
                "Želite li odjaviti vožnju?",
                yesText: "Potvrdi", cancelText: "Odustani");

            if (result is not null)
            {
                if (result.Value)
                {
                    await _autoStoperService.Delete(voznja);

                    _snackBar.Add("Uspješno ste otkazali vožnju", Severity.Success);
                    await DohvatiVoznjeTrenutnogKorisnika();
                }
            }
        }

        public async Task PokaziPutike(Voznja voznja)
        {
            List<string> putnici = (await GetPutniciFromVoznja(voznja.Putnici.Where(p => p.Vozac == false).ToList())).Select(p => p.FirstName + " " + p.LastName).ToList();
            string putniciString = string.Join(", ", putnici);

            await DialogService.ShowMessageBox(
                "Putnici na vožnji",
                putniciString,
                yesText: "U redu", cancelText: "Izlaz");
        }

    }
}
