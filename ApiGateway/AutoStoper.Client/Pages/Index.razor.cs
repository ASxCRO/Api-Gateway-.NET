using ApiGateway.Core.Models.ResponseModels;
using ApiGateway.Core.User;
using AutoStoper.Client.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AutoStoper.Client.Pages
{
    public partial class Index
    {
        public User CurrentUserFullData{ get; set; }
        public List<VoznjaViewModel> SveVoznje { get; set; }
        public List<VoznjaViewModel> SveVoznjeUKojimaTrenutniKorisnikSudjeluje { get; set; }

        public int ObjavljenihVoznji { get; set; }
        public int UkupnoSudjelovanja { get; set; }
        public int UkupnoKorisnikaSKojimaSeTrenutniKorisnikVozio { get; set; }

        protected override async Task OnInitializedAsync()
        {
            SveVoznje = new();
            SveVoznjeUKojimaTrenutniKorisnikSudjeluje = new();
            CurrentUserFullData = new();
            var voznje = await _autoStoperService.GetAll();
            ObjavljenihVoznji = 0;
            UkupnoSudjelovanja = 0;
            UkupnoKorisnikaSKojimaSeTrenutniKorisnikVozio = 0;
            foreach (var item in voznje)
            {
                var newVoznjaViewModel = new VoznjaViewModel
                {
                    Voznja = item,
                    Putnici = await GetPutniciFromVoznja(item.Putnici)
                };
                SveVoznje.Add(newVoznjaViewModel);
                var isDriver = item.Putnici.Any(p => p.UserId == _authorizationService.User.Id && p.Vozac);
                if(isDriver)
                {
                    ObjavljenihVoznji++;
                    UkupnoSudjelovanja++;
                    SveVoznjeUKojimaTrenutniKorisnikSudjeluje.Add(newVoznjaViewModel);
                }
                var isPassenger = item.Putnici.Any(p => p.UserId == _authorizationService.User.Id && p.Vozac == false);
                if (isPassenger)
                {
                    UkupnoSudjelovanja++;
                    SveVoznjeUKojimaTrenutniKorisnikSudjeluje.Add(newVoznjaViewModel);
                }

            }

            foreach(var item in SveVoznjeUKojimaTrenutniKorisnikSudjeluje)
                        foreach (var putnik in item.Putnici.Where(p => p.Id != _authorizationService.User.Id).ToList())
                        {
                                UkupnoKorisnikaSKojimaSeTrenutniKorisnikVozio++;
                        }

             CurrentUserFullData = await _authorizationService.GetById(_authorizationService.User.Id);
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
    }
}
