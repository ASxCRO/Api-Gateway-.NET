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
    public partial class Pretrazi
    {
        [Inject] private IDialogService DialogService { get; set; }

        public Dictionary<int,string> collection { get; set; }
        public int activeKey { get; set; }
        public DateTime? date { get; set; }
        public Lokacija lokacijaPolaziste { get; set; }
        public Lokacija lokacijaOdrediste { get; set; }
        public List<VoznjaViewModel> Voznje { get; set; }
        public bool CantGoBackOnlyReset { get; set; } = false;
        public List<VoznjaViewModel> VoznjeTrenutnogKorisnika { get; set; }

        public bool MojeVoznjeTabActive { get; set; }

        protected override async Task OnInitializedAsync()
        {
            VoznjeTrenutnogKorisnika = new();
            collection = new();
            collection.Add(1, "Datum");
            collection.Add(2, "Polazište");
            collection.Add(3, "Odredište");
            collection.Add(4, "Odaberi prijevoz");

            activeKey = 1;

            await _jsRuntime.InvokeVoidAsync("ResetirajLokacije");
        }

        public async Task SetPreviousItemActive()
        {
            if (activeKey <= 1)
                if (lokacijaPolaziste is null || lokacijaOdrediste is null || !date.HasValue)
                    _snackBar.Add("Prvo ispunite sve ostale podatke", Severity.Error);
                else
                    activeKey = 4;
            else
                activeKey--;

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
                activeKey = 1;
            else
                await GetLatLngPolazisteOdrediste();
                if (activeKey == 3 && (lokacijaPolaziste is null || lokacijaOdrediste is null || !date.HasValue))
                    _snackBar.Add("Prvo ispunite sve ostale podatke", Severity.Error);
                else
                    activeKey++;

            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (MojeVoznjeTabActive == false)
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
                        if (Voznje is null)
                        {
                            Voznje = new();
                            await GetLatLngPolazisteOdrediste();
                            await DohvatiVoznjeUDosegu();
                        }
                        break;
                    default:
                        break;
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
            var voznje = new List<Voznja>();


            foreach (var voznja in sveVoznje)
                foreach (var lokacijaPolaziste in lokacijeKojePasuPolazistu)
                    foreach (var lokacijaOdrediste in lokacijeKojePasuOdredistu)
                            if (lokacijaPolaziste.Lat == voznja.Adresa.LatPolaziste &&
                                lokacijaPolaziste.Lng == voznja.Adresa.LngPolaziste &&
                                lokacijaOdrediste.Lat == voznja.Adresa.LatOdrediste &&
                                lokacijaOdrediste.Lng == voznja.Adresa.LngOdrediste &&
                                date.Value == voznja.DateTime.Date)
                                    voznje.Add(voznja);

            foreach (var item in voznje)
            {
                Voznje.Add(new VoznjaViewModel { 
                    Voznja = item,
                    Putnici = await GetPutniciFromVoznja(item.Putnici)
                });
            }

            CantGoBackOnlyReset = true;
                   
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

        public async Task PotvrdiOdabir(int voznjaId)
        {
            bool? result = await DialogService.ShowMessageBox(
                "Potvrda",
                "Želite li se prijaviti na vožnju?",
                yesText: "Potvrdi", cancelText: "Odustani");

            if (result is not null)
            {
                if (result.Value)
                {
                    var request = new PrijavaNaVoznjuRequest
                    {
                        VoznjaID = voznjaId,
                        UserID = _authorizationService.User.Id
                    };

                    await _autoStoperService.InsertPutnik(request);

                    _snackBar.Add("Uspješna prijava na vožnju!");
                    await Task.Delay(1000);
                    _navigationManager.NavigateTo("/index");
                }
            }
        }

        public async Task PokaziPutike(Voznja voznja)
        {
            List<string> putnici = (await GetPutniciFromVoznja(voznja.Putnici.Where(p=>p.Vozac == false).ToList())).Select(p=>p.FirstName+" "+p.LastName).ToList();
            string putniciString = string.Join(", ", putnici);

            await DialogService.ShowMessageBox(
                "Putnici na vožnji",
                putniciString,
                yesText: "U redu", cancelText: "Izlaz");
        }

        public async Task DohvatiVoznjeTrenutnogKorisnika()
        {
            var sveVoznjeTrenutnogKorisnika = await _autoStoperService.GetByUserId(_authorizationService.User.Id);
            VoznjeTrenutnogKorisnika.Clear();
            foreach (var item in sveVoznjeTrenutnogKorisnika)
                foreach (var putnik in item.Putnici.Where(p=>p.Vozac == false))
                    VoznjeTrenutnogKorisnika.Add(new VoznjaViewModel
                    {
                        Voznja = item,
                        Putnici = await GetPutniciFromVoznja(item.Putnici)
                    });

            MojeVoznjeTabActive = true;
            StateHasChanged();
        }

        public async Task SetFirstTabActive()
        {
            MojeVoznjeTabActive = false;
            StateHasChanged();
        }

        public async Task OtkaziVoznju(Voznja voznja)
        {
            bool? result = await DialogService.ShowMessageBox(
                "Potvrda",
                "Želite li se odjaviti sa vožnje?",
                yesText: "Potvrdi", cancelText: "Odustani");

            if (result is not null)
            {
                if (result.Value)
                {
                    var odjaviPutnika = new PrijavaNaVoznjuRequest {
                        UserID = _authorizationService.User.Id,
                        VoznjaID = voznja.Id
                    };

                    await _autoStoperService.DeletePutnik(odjaviPutnika);

                    _snackBar.Add("Uspješno ste otkazali vožnju", Severity.Success);
                    await DohvatiVoznjeTrenutnogKorisnika();
                }
            }
        }
    }
}
