using ApiGateway.Core.HttpServices;
using ApiGateway.Core.Models.Enums;
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

        public AutoStoperService(IWebAssemblyHttpService webAssemblyHttpService)
        {
            this.webAssemblyHttpService = webAssemblyHttpService;
        }

        public async Task Insert(Voznja voznja)
        {
            await webAssemblyHttpService.Send(Client.ApiGateway, voznja, HttpMethod.Post, $"/novavoznja");
        }

        public async Task<List<Voznja>> GetAll()
        {
            var response = await webAssemblyHttpService.Fetch<List<Voznja>>(Client.ApiGateway, null, HttpMethod.Get, "/voznje");
            return response is not null ? response : null;
        }

        public async Task<Voznja> GetById(int id)
        {
            var response = await webAssemblyHttpService.Fetch<Voznja>(Client.ApiGateway, null, HttpMethod.Get, $"/voznja?id={id}");
            return response is not null ? response : null;
        }

        public async Task Update(Voznja voznja)
        {
            await webAssemblyHttpService.Send(Client.ApiGateway, voznja, HttpMethod.Post, $"/azurirajvoznju");
        }

        public async Task Delete(Voznja voznja)
        {
            await webAssemblyHttpService.Send(Client.ApiGateway, voznja, HttpMethod.Post, $"/obrisivoznju");
        }

        public async Task<List<Voznja>> GetByUserId(int userId)
        {
            var response = await webAssemblyHttpService.Fetch<List<Voznja>>(Client.ApiGateway, null, HttpMethod.Get, $"/voznjekorisnika?userId={userId}");
            return response is not null ? response : null;
        }


    }
}
