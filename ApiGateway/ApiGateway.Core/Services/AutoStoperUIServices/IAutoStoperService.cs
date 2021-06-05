using ApiGateway.Core.Models.RequestModels;
using ApiGateway.Core.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Core.Services.AutoStoperUIServices
{
    public interface IAutoStoperService
    {
        Task Insert(Voznja voznja);
        Task InsertPutnik(PrijavaNaVoznjuRequest prijavaNaVoznjuRequest);
        Task<List<Voznja>> GetAll();
        Task<Voznja> GetById(int id);
        Task<List<Voznja>> GetByUserId(int userId);
        Task Update(Voznja voznja);
        Task Delete(Voznja voznja);
    }
}
