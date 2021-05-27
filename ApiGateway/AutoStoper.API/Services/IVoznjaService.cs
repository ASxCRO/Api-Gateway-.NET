using AutoStoper.API.Data.Database.Models;
using System.Collections.Generic;

namespace AutoStoper.API.Services
{
    public interface IVoznjaService
    {
        IEnumerable<Voznja> GetAll();
        Voznja GetById(int id);
        Voznja GetByUserId(int userID);

    }
}
