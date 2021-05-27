using AutoStoper.API.Data.Database.Models;
using AutoStoper.API.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.API.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Voznja> Voznje { get; }
        IRepository<VoznjaUser> VoznjeUser { get; }
        IRepository<Adresa> Adrese { get; }

        void Commit();
    }
}
