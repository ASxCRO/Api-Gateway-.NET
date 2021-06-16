using ApiGateway.Core.AuthenticationServices;
using ApiGateway.Core.HttpServices;
using ApiGateway.Core.Models.Enums;
using ApiGateway.Core.Models.RequestModels;
using ApiGateway.Core.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Core.Services.AutoStoperUIServices
{
    public class AutoStoperService : IAutoStoperService
    {
        private readonly IWebAssemblyHttpService webAssemblyHttpService;
        private readonly IAuthenticationService authenticationService;

        public AutoStoperService(IWebAssemblyHttpService webAssemblyHttpService, IAuthenticationService authenticationService)
        {
            this.webAssemblyHttpService = webAssemblyHttpService;
            this.authenticationService = authenticationService;
        }

        public async Task Insert(Voznja voznja)
        {
            await webAssemblyHttpService.Send(Client.ApiGateway, voznja, HttpMethod.Post, $"/novavoznja",authenticationService.User?.Token);
        }

        public async Task<List<Voznja>> GetAll()
        {
            var response = await webAssemblyHttpService.Fetch<List<Voznja>>(Client.ApiGateway, null, HttpMethod.Get, "/voznje", authenticationService.User?.Token);
            return response is not null ? response : null;
        }

        public async Task<Voznja> GetById(int id)
        {
            var response = await webAssemblyHttpService.Fetch<Voznja>(Client.ApiGateway, null, HttpMethod.Get, $"/voznja?id={id}",authenticationService.User?.Token);
            return response is not null ? response : null;
        }

        public async Task Update(Voznja voznja)
        {
            await webAssemblyHttpService.Send(Client.ApiGateway, voznja, HttpMethod.Post, $"/azurirajvoznju", authenticationService.User?.Token);
        }

        public async Task Delete(Voznja voznja)
        {
            await webAssemblyHttpService.Send(Client.ApiGateway, voznja, HttpMethod.Post, $"/obrisivoznju",authenticationService.User?.Token);
        }

        public async Task<List<Voznja>> GetByUserId(int userId)
        {
            var response = await webAssemblyHttpService.Fetch<List<Voznja>>(Client.ApiGateway, null, HttpMethod.Get, $"/voznjekorisnika?userId={userId}",authenticationService.User?.Token);
            return response is not null ? response : null;
        }

        public async Task InsertPutnik(PrijavaNaVoznjuRequest prijavaNaVoznjuRequest)
        {
            await webAssemblyHttpService.Send(Client.ApiGateway, prijavaNaVoznjuRequest, HttpMethod.Post, $"/dodajputnika",authenticationService.User?.Token);
        }

        public async Task DeletePutnik(PrijavaNaVoznjuRequest prijavaNaVoznjuRequest)
        {
            await webAssemblyHttpService.Send(Client.ApiGateway, prijavaNaVoznjuRequest, HttpMethod.Post, $"/makniputnika",authenticationService.User?.Token);
        }
    }
}
