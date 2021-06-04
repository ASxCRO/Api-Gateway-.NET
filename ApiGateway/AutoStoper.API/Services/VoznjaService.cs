using AutoStoper.API.Data.Database.Models;
using AutoStoper.API.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoStoper.API.Services
{
    public class VoznjaService : IVoznjaService
    {
        private readonly IUnitOfWork unitOfWork;

        public VoznjaService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool Delete(Voznja voznja)
        {
            try
            {
                unitOfWork.Voznje.Delete(voznja.Id);
                unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Voznja> GetAll()
        {
            return unitOfWork.Voznje.Get(includeProperties: "Putnici,Adresa");
        }

        public Voznja GetById(int id)
        {
            return unitOfWork.Voznje.Get(p=>p.Id == id, includeProperties: "Putnici,Adresa").FirstOrDefault();
        }

        public Voznja GetByUserId(int userID)
        {
            var voznja = unitOfWork.Voznje.Get(v => v.Putnici.Any(p=>p.UserId == userID), includeProperties: "Putnici,Adresa").FirstOrDefault();
            return voznja;
        }

        public bool Insert(Voznja voznja)
        {
            try
            {
                unitOfWork.Voznje.Insert(voznja);
                unitOfWork.Adrese.Insert(voznja.Adresa);

                unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Voznja voznja)
        {
            try
            {
                unitOfWork.Voznje.Update(voznja);
                unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
