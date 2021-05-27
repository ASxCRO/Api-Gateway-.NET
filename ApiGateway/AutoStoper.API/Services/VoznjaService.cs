using AutoStoper.API.Data.Database.Models;
using AutoStoper.API.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.API.Services
{
    public class VoznjaService : IVoznjaService
    {
        private readonly IUnitOfWork unitOfWork;

        public VoznjaService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Voznja> GetAll()
        {
            return unitOfWork.Voznje.Get();
        }

        public Voznja GetById(int id)
        {
            return unitOfWork.Voznje.GetByID(id);
        }

        public Voznja GetByUserId(int userID)
        {
            var korisnikVoznja = unitOfWork.VoznjeUser.Get(v => v.UserId == userID).FirstOrDefault();
            return korisnikVoznja?.Voznja;
        }
    }
}
