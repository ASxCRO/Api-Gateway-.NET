using ApiGateway.Core.Models.RequestModels;
using AutoStoper.API.Data.Database.Models;
using System.Collections.Generic;

namespace AutoStoper.API.Services
{
    public interface IVoznjaService
    {
        bool Insert(Voznja voznja);
        bool InsertPutnika(PrijavaNaVoznjuRequest prijavaNaVoznjuRequest);
        bool DeletePutnika(PrijavaNaVoznjuRequest prijavaNaVoznjuRequest);
        IEnumerable<Voznja> GetAll();
        Voznja GetById(int id);
        List<Voznja> GetByUserId(int userID);
        bool Delete(Voznja voznja);
        bool Update(Voznja voznja);

    }
}
