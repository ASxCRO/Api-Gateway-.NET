using AutoStoper.API.Data.Database.Models;
using System.Collections.Generic;

namespace AutoStoper.API.Services
{
    public interface IVoznjaService
    {
        bool Insert(Voznja voznja);
        IEnumerable<Voznja> GetAll();
        Voznja GetById(int id);
        Voznja GetByUserId(int userID);
        bool Delete(Voznja voznja);
        bool Update(Voznja voznja);

    }
}
